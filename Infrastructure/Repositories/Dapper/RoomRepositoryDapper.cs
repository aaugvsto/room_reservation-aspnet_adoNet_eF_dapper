using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.Entities;
using Infrastructure.DataAccess.Dapper;

namespace Infrastructure.Repositories
{
    public class RoomRepositoryDapper : RoomRepository
    {
        public readonly IDbConnectionFactoryDapper _dbConnection;

        public RoomRepositoryDapper(IDbConnectionFactoryDapper dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async override Task<Room> Add(Room entity)
        {
            entity.CreationDate = DateTime.UtcNow;
            entity.ModifiedDate = DateTime.UtcNow;

            using (var connection = _dbConnection.CreateConnection())
            {
                var sql =
                    @"INSERT INTO ROOM (NAME, CAPACITY, LOCATION, CREATION_DATE, MODIFICATION_DATE) 
                            VALUES (@Name, @Capacity, @Location, @CreationDate, @ModificationDate);
                            SELECT last_insert_rowid();";

                entity.Id = await connection.ExecuteScalarAsync<int>(sql, new { entity.Name, entity.Capacity, entity.Location, entity.CreationDate, entity.ModifiedDate });
            }

            return entity;
        }

        public async override Task<bool> Delete(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = "DELETE FROM ROOM WHERE ID = @Id";

                var result = connection.ExecuteAsync(sql, new { Id = id });

                return await result > 0;
            }
        }

        public async override Task<IEnumerable<Room>> GetAll()
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = "SELECT ID,NAME,CAPACITY,LOCATION,CREATION_DATE,MODIFICATION_DATE FROM ROOM";

                return await connection.QueryAsync<Room>(sql);
            }
        }

        public async override Task<Room> GetById(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = "SELECT ID,NAME,CAPACITY,LOCATION,CREATION_DATE,MODIFICATION_DATE FROM ROOM WHERE ID = @id";

                return await connection.QuerySingleOrDefaultAsync<Room>(sql, new { id = id });
            }
        }

        public async override Task<Room> Update(Room entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;

            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = @"UPDATE ROOM 
                            SET NAME = @Name, Capacity = @Capacity, Location = @Location, MODIFICATION_DATE = @ModificationDate
                            WHERE ID = @Id";

                var result = await connection.ExecuteAsync(sql, new { entity.Name, entity.Capacity, entity.Location, entity.ModifiedDate, entity.Id });

                return entity;
            }
        }
    }
}
