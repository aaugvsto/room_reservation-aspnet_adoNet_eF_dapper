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
    public class ReservationRepositoryADO : ReservationRepository
    {
        private readonly IDbConnectionFactoryADONET _dbConnection;

        public ReservationRepositoryADO(IDbConnectionFactoryADONET dbConnection)
        {
            _dbConnection = dbConnection;
        }


        public async override Task<Reservation> Add(Reservation entity)
        {
            entity.CreationDate = DateTime.UtcNow;
            entity.ModifiedDate = DateTime.UtcNow;

            using (var dbCon = _dbConnection.CreateConnection())
            {
                dbCon.Open();

                var command = dbCon.CreateCommand();

                command.CommandText =
                    @"
                        INSERT INTO RESERVATION
                        (
                            ID_EMPLOYEE,
                            ID_ROOM,
                            CREATION_DATE,
                            MODIFICATION_DATE
                        )
                        VALUES
                        (
                            @IdEmployee,
                            @IdRoom,
                            @creationDate,
                            @modificationDate
                        );
                        SELECT last_insert_rowid();
                    ";
                command.Parameters.Add(new SqliteParameter("@IdEmployee", entity.EmployeeId));
                command.Parameters.Add(new SqliteParameter("@IdRoom", entity.RoomId));
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
                        DELETE FROM RESERVATION
                        WHERE ID = @id
                    ";
                command.Parameters.Add(new SqliteParameter("@id", id));

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async override Task<IEnumerable<Reservation>> GetAll()
        {
            var Reservations = new List<Reservation>();

            using (var dbCon = _dbConnection.CreateConnection())
            {
                dbCon.Open();

                var command = dbCon.CreateCommand();

                command.CommandText =
                    @"
                        SELECT
                            ID,
                            ID_EMPLOYEE,
                            ID_ROOM,
                            CREATION_DATE,
                            MODIFICATION_DATE
                        FROM 
                            RESERVATION
                    ";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var reservation = new Reservation
                        {
                            Id = reader.GetInt32(0),
                            EmployeeId = reader.GetInt32(1),
                            RoomId = reader.GetInt32(2),
                            CreationDate = DateTime.Parse(reader.GetString(3)),
                            ModifiedDate = DateTime.Parse(reader.GetString(4)),
                        };

                        Reservations.Add(reservation);
                    }
                }
            }

            return Reservations;
        }

        public async override Task<Reservation> GetById(int id)
        {
            var reservation = new Reservation();

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
                            RESERVATION
                        WHERE ID = @id
                    ";

                command.Parameters.Add(new SqliteParameter("@id", id));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        reservation = new Reservation
                        {
                            Id = reader.GetInt32(0),
                            EmployeeId = reader.GetInt32(1),
                            RoomId = reader.GetInt32(2),
                            CreationDate = DateTime.Parse(reader.GetString(4)),
                            ModifiedDate = DateTime.Parse(reader.GetString(5)),
                        };
                    }
                }

                return reservation;
            }
        }

        public async override Task<Reservation> Update(Reservation entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;

            using (var dbCon = _dbConnection.CreateConnection())
            {
                dbCon.Open();
                var command = dbCon.CreateCommand();
                command.CommandText =
                    @"
                        UPDATE RESERVATION
                        SET 
                            ID_EMPLOYEE = @idEmployee,
                            ID_ROOM = @idRoom,
                            MODIFICATION_DATE = @modificationDate
                        WHERE ID = @id
                    ";

                command.Parameters.Add(new SqliteParameter("@idEmployee", entity.EmployeeId));
                command.Parameters.Add(new SqliteParameter("@idRoom", entity.RoomId));
                command.Parameters.Add(new SqliteParameter("@modificationDate", entity.ModifiedDate));
                command.Parameters.Add(new SqliteParameter("@id", entity.Id));

                await command.ExecuteNonQueryAsync();

                return entity;
            }
        }
    }
}
