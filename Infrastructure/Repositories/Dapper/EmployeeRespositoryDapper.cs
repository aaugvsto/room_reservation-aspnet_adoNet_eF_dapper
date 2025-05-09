using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.DataAccess.Dapper;
using Dapper;

namespace Infrastructure.Repositories
{
    public class EmployeeRespositoryDapper : EmployeeRespository
    {
        private readonly IDbConnectionFactoryDapper _dbConnectionFactory;

        public EmployeeRespositoryDapper(IDbConnectionFactoryDapper dbConnectionFactory) 
        { 
            _dbConnectionFactory = dbConnectionFactory;
        }

        public override async Task<Employee> Add(Employee entity)
        {
            entity.CreationDate = DateTime.UtcNow;
            entity.ModifiedDate = DateTime.UtcNow;

            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var sql =
                    @"INSERT INTO Employees (NAME, EMAIL, DEPARTMENT, CREATION_DATE, MODIFICATION_DATE) 
                            VALUES (@Name, @Email, @Department, @CreationDate, @ModificationDate);
                            SELECT last_insert_rowid();";

                entity.Id = await connection.ExecuteScalarAsync<int>(sql, new { entity.Name, entity.Email, entity.Department, entity.CreationDate, entity.ModifiedDate });
            }

            return entity;
        }

        public async override Task<bool> Delete(int id)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var sql = "DELETE FROM EMPLOYEE WHERE ID = @Id";

                var result = connection.ExecuteAsync(sql, new { Id = id });

                return await result > 0;
            }
        }

        public async override Task<IEnumerable<Employee>> GetAll()
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var sql = "SELECT * FROM EMPLOYEE";

                return await connection.QueryAsync<Employee>(sql);
            }
        }

        public async override Task<Employee> GetById(int id)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var sql = "SELECT * FROM EMPLOYEE WHERE ID = @Id";
                
                return await connection.QuerySingleOrDefaultAsync<Employee>(sql, new { Id = id });
            }
        }

        public async override Task<Employee> Update(Employee entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;

            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var sql = @"UPDATE EMPLOYEE 
                            SET NAME = @Name, EMAIL = @Email, DEPARTMENT = @Department, MODIFICATION_DATE = @ModificationDate
                            WHERE ID = @Id";

                var result = await connection.ExecuteAsync(sql, new { entity.Name, entity.Email, entity.Department, entity.ModifiedDate, entity.Id });
                
                return entity;
            }
        }
    }
}
