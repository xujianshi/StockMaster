using Chloe.Infrastructure;
using System.Data;
using System.Data.SQLite;

namespace DataAccess.DbConnectionFactory
{
    public class SQLiteConnectionFactory : IDbConnectionFactory
    {
        private string _connString = null;

        public SQLiteConnectionFactory(string connString)
        {
            this._connString = connString;
        }

        public IDbConnection CreateConnection()
        {
            SQLiteConnection conn = new SQLiteConnection(this._connString);
            return conn;
        }
    }
}