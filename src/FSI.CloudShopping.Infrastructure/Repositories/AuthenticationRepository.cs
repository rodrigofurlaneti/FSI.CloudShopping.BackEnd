using System.Data;
using Microsoft.Data.SqlClient;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Infrastructure.Data;
using FSI.CloudShopping.Infrastructure.Security;

namespace FSI.CloudShopping.Infrastructure.Repositories
{
    public class AuthenticationRepository : BaseRepository<Authentication>, IAuthenticationRepository
    {
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationRepository(SqlDbConnector connector, IPasswordHasher passwordHasher)
            : base(connector)
        {
            _passwordHasher = passwordHasher;
        }

        protected override string ProcInsert => "Procedure_Authentication_Insert";
        protected override string ProcUpdate => string.Empty;
        protected override string ProcDelete => string.Empty;
        protected override string ProcGetById => string.Empty;
        protected override string ProcGetAll => string.Empty;

        public override Task AddAsync(Authentication entity) => throw new NotImplementedException();
        public override Task UpdateAsync(Authentication entity) => throw new NotImplementedException();
        public async Task<int> InsertAsync(string email, bool isAuthorized)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcInsert);
            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = email;
            cmd.Parameters.Add("@AuthorizedAccess", SqlDbType.Bit).Value = isAuthorized;
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }
        public async Task<bool> GetAccessAsync(string email, string password)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync("SELECT 1");
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT [PasswordHash] FROM [dbo].[Customers] WHERE [Email] = @Email AND [IsActive] = 1";
            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = email;
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var storedHash = reader["PasswordHash"].ToString();
                return _passwordHasher.VerifyPassword(password, storedHash ?? string.Empty);
            }
            return false;
        }
        protected override Authentication MapToEntity(SqlDataReader reader)
        {
            return new Authentication(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader["Email"].ToString() ?? string.Empty,
                reader.GetBoolean(reader.GetOrdinal("AuthorizedAccess")),
                reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            );
        }
    }
}