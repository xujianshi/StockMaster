using Chloe.Infrastructure;
using Npgsql;
using System.Data;

namespace DataAccess.DbConnectionFactory
{
    public class PostgreSQLConnectionFactory : IDbConnectionFactory
    {
        private string _connString = null;

        public PostgreSQLConnectionFactory(string connString)
        {
            this._connString = connString;
        }

        public IDbConnection CreateConnection()
        {
            NpgsqlConnection conn = new NpgsqlConnection(this._connString);
            return conn;
        }
    }
}