using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.DataAccess.ADONET;
using Infrastructure.DataAccess.EntityFramework.Mapping;
using Microsoft.Data.Sqlite;

namespace Infrastructure.Repositories
{
    public class EmployeeRespositoryADO : EmployeeRespository
    {
        private readonly IDbConnectionFactoryADONET _dbConnection;

        public EmployeeRespositoryADO(IDbConnectionFactoryADONET dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public override async Task<Employee> Add(Employee entity)
        {
            entity.CreationDate = DateTime.UtcNow;
            entity.ModifiedDate = DateTime.UtcNow;

            using (var dbCon = _dbConnection.CreateConnection())
            {
                dbCon.Open();

                var command = dbCon.CreateCommand();

                command.CommandText =
                    @"
                        INSERT INTO EMPLOYEE
                        (
                            NAME,
                            EMAIL,
                            DEPARTMENT,
                            CREATION_DATE,
                            MODIFICATION_DATE
                        )
                        VALUES
                        (
                            @name,
                            @email,
                            @department,
                            @creationDate,
                            @modificationDate
                        );
                        SELECT last_insert_rowid();
                    ";
                command.Parameters.Add(new SqliteParameter("@name", entity.Name));
                command.Parameters.Add(new SqliteParameter("@email", entity.Email));
                command.Parameters.Add(new SqliteParameter("@department", entity.Department.ToString()));
                command.Parameters.Add(new SqliteParameter("@creationDate", entity.CreationDate));
                command.Parameters.Add(new SqliteParameter("@modificationDate", entity.ModifiedDate));

                var id = (long)await command.ExecuteScalarAsync();

                entity.Id = (int)id;

                return entity;
            }
        }

        public override Task<bool> Delete(int id)
        {
            using (var dbCon = _dbConnection.CreateConnection())
            { 
                dbCon.Open();
                var command = dbCon.CreateCommand();
                command.CommandText =
                    @"
                        DELETE FROM EMPLOYEE
                        WHERE ID = @id
                    ";
                command.Parameters.Add(new SqliteParameter("@id", id));
                return Task.FromResult(command.ExecuteNonQuery() > 0);
            }
        }

        public async override Task<IEnumerable<Employee>> GetAll()
        {
            Dictionary<int, Employee> employeeMap = new Dictionary<int, Employee>();

            using (var dbCon = _dbConnection.CreateConnection())
            {
                dbCon.Open();

                var command = dbCon.CreateCommand();

                command.CommandText =
                    @"
                        SELECT
                            E.ID,
                            E.NAME,
                            E.EMAIL,
                            E.DEPARTMENT,
                            E.CREATION_DATE,
                            E.MODIFICATION_DATE,
                            R.ID, 
                            R.ID_ROOM,
                            R.CREATION_DATE,
                            R.MODIFICATION_DATE,
                            RR.ID,
                            RR.NAME,    
                            RR.CAPACITY,
                            RR.LOCATION,
                            RR.CREATION_DATE,
                            RR.MODIFICATION_DATE
                        FROM 
                            EMPLOYEE E
                        LEFT JOIN
                            RESERVATION R ON R.ID_EMPLOYEE = E.ID
                        LEFT JOIN
                            ROOM RR ON RR.ID = R.ID_ROOM   
                    ";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var employeeId = reader.GetInt32(0);

                        if (!employeeMap.ContainsKey(employeeId))
                        {
                            employeeMap.Add(employeeId, new Employee
                            {
                                Id = employeeId,
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Department = Enum.Parse<Department>(reader.GetString(3)),
                                CreationDate = reader.GetDateTime(4),
                                ModifiedDate = reader.GetDateTime(5),
                            });
                        }

                        if (!reader.IsDBNull(6))
                        {
                            var reservation = new Reservation
                            {
                                Id = reader.GetInt32(6),
                                EmployeeId = employeeId,
                                RoomId = reader.GetInt32(7),
                                CreationDate = reader.GetDateTime(8),
                                ModifiedDate = reader.GetDateTime(9),
                                Room = new Room
                                {
                                    Id = reader.GetInt32(10),
                                    Name = reader.GetString(11),
                                    Capacity = reader.GetInt32(12),
                                    Location = Enum.Parse<Location>(reader.GetString(13)),
                                    CreationDate = reader.GetDateTime(14),
                                    ModifiedDate = reader.GetDateTime(15),
                                },
                            };

                            employeeMap[employeeId]
                                .Reservations
                                .Add(reservation);
                        }
                    }
                }
            }

            return employeeMap.Values;
        }

        public override async Task<Employee> GetById(int id)
        {
            Employee? employee = null;

            using (var dbCon = _dbConnection.CreateConnection())
            {
                dbCon.Open();

                var command = dbCon.CreateCommand();

                command.CommandText =
                    @"
                        SELECT
                            E.ID,
                            E.NAME,
                            E.EMAIL,
                            E.DEPARTMENT,
                            E.CREATION_DATE,
                            E.MODIFICATION_DATE,
                            R.ID, 
                            R.ID_ROOM,
                            R.CREATION_DATE,
                            R.MODIFICATION_DATE,
                            RR.ID,
                            RR.NAME,    
                            RR.CAPACITY,
                            RR.LOCATION,
                            RR.CREATION_DATE,
                            RR.MODIFICATION_DATE
                        FROM 
                            EMPLOYEE E
                        LEFT JOIN
                            RESERVATION R ON R.ID_EMPLOYEE = E.ID
                        LEFT JOIN
                            ROOM RR ON RR.ID = R.ID_ROOM 
                        WHERE E.ID = @id
                    ";

                command.Parameters.Add(new SqliteParameter("@id", id));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if(employee is null)
                        {
                            employee = new Employee
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Department = Enum.Parse<Department>(reader.GetString(3)),
                                CreationDate = reader.GetDateTime(4),
                                ModifiedDate = reader.GetDateTime(5),
                            };
                        }

                        if(employee is Employee && !reader.IsDBNull(6))
                        {
                            var reservation = new Reservation
                            {
                                Id = reader.GetInt32(6),
                                EmployeeId = employee.Id,
                                RoomId = reader.GetInt32(7),
                                CreationDate = reader.GetDateTime(8),
                                ModifiedDate = reader.GetDateTime(9),
                                Room = new Room
                                {
                                    Id = reader.GetInt32(10),
                                    Name = reader.GetString(11),
                                    Capacity = reader.GetInt32(12),
                                    Location = Enum.Parse<Location>(reader.GetString(13)),
                                    CreationDate = reader.GetDateTime(14),
                                    ModifiedDate = reader.GetDateTime(15),
                                },
                            };

                            employee.Reservations.Add(reservation);
                        }
                    }
                }

                return employee;
            }
        }

        public override async Task<Employee> Update(Employee entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;

            using (var dbCon = _dbConnection.CreateConnection())
            {
                dbCon.Open();
                var command = dbCon.CreateCommand();
                command.CommandText =
                    @"
                        UPDATE EMPLOYEE
                        SET 
                            NAME = @name,
                            EMAIL = @email,
                            DEPARTMENT = @department,
                            MODIFICATION_DATE = @modificationDate
                        WHERE ID = @id
                    ";

                command.Parameters.Add(new SqliteParameter("@name", entity.Name));
                command.Parameters.Add(new SqliteParameter("@email", entity.Email));
                command.Parameters.Add(new SqliteParameter("@department", entity.Department.ToString()));
                command.Parameters.Add(new SqliteParameter("@modificationDate", entity.ModifiedDate));
                command.Parameters.Add(new SqliteParameter("@id", entity.Id));

                await command.ExecuteNonQueryAsync();

                return entity;
            }
        }
    }
}
