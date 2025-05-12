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
            var rooms = new List<Room>();

            using (var dbCon = _dbConnection.CreateConnection())
            {
                dbCon.Open();

                var command = dbCon.CreateCommand();

                command.CommandText =
                    @"
                        SELECT
                            ID,
                            NAME,
                            CAPACITY,
                            LOCATION,
                            CREATION_DATE,
                            MODIFICATION_DATE
                        FROM 
                            ROOM
                    ";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var room = new Room
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Capacity = reader.GetInt32(2),
                            Location = Enum.Parse<Location>(reader.GetString(3)),
                            CreationDate = DateTime.Parse(reader.GetString(4)),
                            ModifiedDate = DateTime.Parse(reader.GetString(5)),
                        };

                        rooms.Add(room);
                    }
                }
            }

            return rooms;
        }

        public async override Task<Room> GetById(int id)
        {
            var room = new Room();

            using (var dbCon = _dbConnection.CreateConnection())
            {
                dbCon.Open();

                var command = dbCon.CreateCommand();

                command.CommandText =
                    @"
                        SELECT
                            ID,
                            NAME,
                            CAPACITY,
                            LOCATION,
                            CREATION_DATE,
                            MODIFICATION_DATE
                        FROM 
                            ROOM
                        WHERE ID = @id
                    ";

                command.Parameters.Add(new SqliteParameter("@id", id));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
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
