using System;
using MySql.Data.MySqlClient;

namespace FileUpload
{
    public class DbContext : IDisposable
    {
        public readonly MySqlConnection _connection;

        public DbContext(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
