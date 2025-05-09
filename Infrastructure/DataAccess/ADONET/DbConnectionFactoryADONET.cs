using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Infrastructure.DataAccess.ADONET
{
    public class DbConnectionFactoryADONET : IDbConnectionFactoryADONET
    {
        private readonly string _connectionString;

        public DbConnectionFactoryADONET(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}
