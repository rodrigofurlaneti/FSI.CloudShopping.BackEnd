using System.Data;
using Microsoft.Data.SqlClient;
namespace FSI.CloudShopping.Infrastructure.Data
{
    public class SqlDbConnector : IDisposable
    {
        private readonly SqlConnection _connection;
        public SqlDbConnector(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("A ConnectionString não pode ser nula.");

            _connection = new SqlConnection(connectionString);
        }
        public async Task<SqlCommand> CreateProcedureCommandAsync(string name)
        {
            if (_connection.State == ConnectionState.Closed)
                await _connection.OpenAsync();
            return new SqlCommand(name, _connection)
            {
                CommandType = CommandType.StoredProcedure
            };
        }
        public void Dispose()
        {
            if (_connection.State != ConnectionState.Closed)
                _connection.Close();
            _connection.Dispose();
        }
    }
}