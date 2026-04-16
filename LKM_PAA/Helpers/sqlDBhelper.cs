using Npgsql;
using System.Data;

namespace LKM_PAA.Helpers
{
    public class SqlDBHelper
    {
        private readonly string _connectionString;

        public SqlDBHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public NpgsqlCommand GetCommand(NpgsqlConnection conn, string query)
        {
            return new NpgsqlCommand(query, conn);
        }
    }
}