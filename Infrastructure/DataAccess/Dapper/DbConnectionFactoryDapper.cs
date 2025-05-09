using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Infrastructure.DataAccess.Dapper
{
    public class DbConnectionFactoryDapper : IDbConnectionFactoryDapper
    {
        private readonly string _connectionString;

        public DbConnectionFactoryDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}
