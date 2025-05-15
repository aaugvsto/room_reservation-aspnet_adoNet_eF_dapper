using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.DataAccess.ADONET;
using Microsoft.Data.Sqlite;

namespace Infrastructure.Repositories
{
    public class RoomRepositoryADO : RoomRepository
    {
        private readonly IDbConnectionFactoryADONET _dbConnection;

        public RoomRepositoryADO(IDbConnectionFactoryADONET dbConnection)
        {
            _dbConnection = dbConnection;
        }


        public async override Task<Room> Add(Room entity)
        {
            entity.CreationDate = DateTime.UtcNow;
            entity.ModifiedDate = DateTime.UtcNow;

            using (var dbCon = _dbConnection.CreateConnection())
            {
                dbCon.Open();

                var command = dbCon.CreateCommand();

                command.CommandText =
                    @"
                        INSERT INTO ROOM
                        (
                            NAME,
                            CAPACITY,
                            LOCATION,
                            CREATION_DATE,
                            MODIFICATION_DATE
                        )
                        VALUES
                        (
                            @name,
                            @capacity,
                            @location,
                            @creationDate,
                            @modificationDate
                        );
                        SELECT last_insert_rowid();
                    ";
                command.Parameters.Add(new SqliteParameter("@name", entity.Name));
                command.Parameters.Add(new SqliteParameter("@capacity", entity.Capacity));
                command.Parameters.Add(new SqliteParameter("@location", entity.Location.ToString()));
                command.Parameters.Add(new SqliteParameter("@creationDate", entity.CreationDate));
                command.Parameters.Add(new SqliteParameter("@modificationDate", entity.ModifiedDate));

                var id = (long)await command.ExecuteScalarAsync();

                entity.Id = (int)id;

                return entity;
            }
        }

        public async override Task<bool> Delete(int id)
        {
            using (var dbCon = _dbConnection.CreateConnection())
            {
                dbCon.Open();

                var command = dbCon.CreateCommand();

                command.CommandText =
                    @"
                        DELETE FROM ROOM
                        WHERE ID = @id
                    ";
                command.Parameters.Add(new SqliteParameter("@id", id));

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async override Task<IEnumerable<Room>> GetAll()
        {
            Dictionary<int, Room> rooms = new Dictionary<int, Room>();

            using (var dbCon = _dbConnection.CreateConnection())
            {
                dbCon.Open();

                var command = dbCon.CreateCommand();

                command.CommandText =
                    @"
                        SELECT
                            R.ID,
                            R.NAME,
                            R.CAPACITY,
                            R.LOCATION,
                            R.CREATION_DATE,
                            R.MODIFICATION_DATE,
                            RR.ID,
                            RR.ID_EMPLOYEE,
                            RR.CREATION_DATE,
                            RR.MODIFICATION_DATE
                        FROM 
                            ROOM R
                        LEFT JOIN 
                            RESERVATION RR ON R.ID = RR.ID_ROOM
                    ";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var idRoom = reader.GetInt32(0);
                        if (!rooms.ContainsKey(idRoom))
                        {
                            rooms.Add(idRoom, new Room
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Capacity = reader.GetInt32(2),
                                Location = Enum.Parse<Location>(reader.GetString(3)),
                                CreationDate = DateTime.Parse(reader.GetString(4)),
                                ModifiedDate = DateTime.Parse(reader.GetString(5))
                            });
                        }

                        if (!reader.IsDBNull(6))
                        {
                            var reservation = new Reservation{
                                Id = reader.GetInt32(6),
                                RoomId = idRoom,
                                EmployeeId = reader.GetInt32(7),
                                CreationDate = DateTime.Parse(reader.GetString(8)),
                                ModifiedDate = DateTime.Parse(reader.GetString(9))
                            };

                            rooms[idRoom].Reservations.Add(reservation);
                        }
                    }
                }
            }

            return rooms.Values;
        }

        public async override Task<Room> GetById(int id)
        {
            Room? room = null;

            using (var dbCon = _dbConnection.CreateConnection())
            {
                dbCon.Open();

                var command = dbCon.CreateCommand();

                command.CommandText =
                    @"
                        SELECT
                            R.ID,
                            R.NAME,
                            R.CAPACITY,
                            R.LOCATION,
                            R.CREATION_DATE,
                            R.MODIFICATION_DATE,
                            RR.ID,
                            RR.ID_EMPLOYEE,
                            RR.CREATION_DATE,
                            RR.MODIFICATION_DATE
                        FROM 
                            ROOM R
                        LEFT JOIN 
                            RESERVATION RR ON R.ID = RR.ID_ROOM
                        WHERE ID = @id
                    ";

                command.Parameters.Add(new SqliteParameter("@id", id));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (room is null)
                        {
                            room = new Room
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Capacity = reader.GetInt32(2),
                                Location = Enum.Parse<Location>(reader.GetString(3)),
                                CreationDate = DateTime.Parse(reader.GetString(4)),
                                ModifiedDate = DateTime.Parse(reader.GetString(5)),
                            };
                        }

                        if (!reader.IsDBNull(6))
                        {
                            var reservation = new Reservation
                            {
                                Id = reader.GetInt32(6),
                                RoomId = id,
                                EmployeeId = reader.GetInt32(7),
                                CreationDate = DateTime.Parse(reader.GetString(8)),
                                ModifiedDate = DateTime.Parse(reader.GetString(9))
                            };

                            room.Reservations.Add(reservation);
                        }
                    }
                }

                return room;
            }
        }

        public async override Task<Room> Update(Room entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;

            using (var dbCon = _dbConnection.CreateConnection())
            {
                dbCon.Open();
                var command = dbCon.CreateCommand();
                command.CommandText =
                    @"
                        UPDATE ROOM
                        SET 
                            NAME = @name,
                            CAPACITY = @capacity,
                            LOCATION = @location,
                            MODIFICATION_DATE = @modificationDate
                        WHERE ID = @id
                    ";

                command.Parameters.Add(new SqliteParameter("@name", entity.Name));
                command.Parameters.Add(new SqliteParameter("@capacity", entity.Capacity));
                command.Parameters.Add(new SqliteParameter("@location", entity.Location.ToString()));
                command.Parameters.Add(new SqliteParameter("@modificationDate", entity.ModifiedDate));
                command.Parameters.Add(new SqliteParameter("@id", entity.Id));

                await command.ExecuteNonQueryAsync();

                return entity;
            }
        }
    }
}
