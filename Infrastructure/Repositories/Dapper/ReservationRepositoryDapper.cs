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
    public class ReservationRepositoryDapper : ReservationRepository
    {
        public readonly IDbConnectionFactoryDapper _dbConnection;

        public ReservationRepositoryDapper(IDbConnectionFactoryDapper dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async override Task<Reservation> Add(Reservation entity)
        {
            entity.CreationDate = DateTime.UtcNow;
            entity.ModifiedDate = DateTime.UtcNow;

            using (var connection = _dbConnection.CreateConnection())
            {
                var sql =
                    @"INSERT INTO RESERVATION (ID_EMPLOYEE, ID_ROOM, CREATION_DATE, MODIFICATION_DATE) 
                            VALUES (@Name, @Capacity, @Location, @CreationDate, @ModificationDate);
                            SELECT last_insert_rowid();";

                entity.Id = await connection.ExecuteScalarAsync<int>(sql, new { entity.EmployeeId, entity.RoomId, entity.CreationDate, entity.ModifiedDate });
            }

            return entity;
        }

        public async override Task<bool> Delete(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = "DELETE FROM RESERVATION WHERE ID = @Id";

                var result = connection.ExecuteAsync(sql, new { Id = id });

                return await result > 0;
            }
        }

        public async override Task<IEnumerable<Reservation>> GetAll()
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = "SELECT ID,ID_EMPLOYEE, ID_ROOM, CREATION_DATE, MODIFICATION_DATE FROM ROOM";

                return await connection.QueryAsync<Reservation>(sql);
            }
        }

        public async override Task<Reservation> GetById(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = "SELECT ID,ID_EMPLOYEE, ID_ROOM, CREATION_DATE, MODIFICATION_DATE FROM ROOM WHERE ID = @id";

                return await connection.QuerySingleOrDefaultAsync<Reservation>(sql, new { id = id });
            }
        }

        public async override Task<Reservation> Update(Reservation entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;

            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = @"UPDATE RESERVATION 
                            SET ID_EMPLOYEE = @EmployeeId, ID_ROOM = @RoomId, MODIFICATION_DATE = @ModificationDate
                            WHERE ID = @Id";

                var result = await connection.ExecuteAsync(sql, new { entity.EmployeeId, entity.RoomId, entity.ModifiedDate, entity.Id });

                return entity;
            }
        }
    }
}
