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
                entity.CreationDate = DateTime.UtcNow;
                entity.ModifiedDate = DateTime.UtcNow;

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
            var employees = new List<Employee>();

            using (var dbCon = _dbConnection.CreateConnection())
            {
                dbCon.Open();

                var command = dbCon.CreateCommand();

                command.CommandText =
                    @"
                        SELECT
                            ID,
                            NAME,
                            EMAIL,
                            DEPARTMENT,
                            CREATION_DATE,
                            MODIFICATION_DATE
                        FROM 
                            EMPLOYEE
                    ";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var employee = new Employee
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            Department = Enum.Parse<Department>(reader.GetString(3)),
                            CreationDate = reader.GetDateTime(4),
                            ModifiedDate = reader.GetDateTime(5),
                        };

                        employees.Add(employee);
                    }
                }
            }

            return employees;
        }

        public override async Task<Employee> GetById(int id)
        {
            var employee = new Employee();

            using (var dbCon = _dbConnection.CreateConnection())
            {
                dbCon.Open();

                var command = dbCon.CreateCommand();

                command.CommandText =
                    @"
                        SELECT
                            ID,
                            NAME,
                            EMAIL,
                            DEPARTMENT,
                            CREATION_DATE,
                            MODIFICATION_DATE
                        FROM 
                            EMPLOYEE
                        WHERE ID = @id
                    ";

                command.Parameters.Add(new SqliteParameter("@id", id));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
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
                }

                return employee;
            }
        }

        public override async Task<Employee> Update(Employee entity)
        {
            using(var dbCon = _dbConnection.CreateConnection())
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
                command.Parameters.Add(new SqliteParameter("@modificationDate", DateTime.UtcNow));
                command.Parameters.Add(new SqliteParameter("@id", entity.Id));

                await command.ExecuteNonQueryAsync();
                return entity;
            }
        }
    }
}
