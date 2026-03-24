namespace FSI.CloudShopping.Infrastructure.Repositories;

using System.Data;
using Microsoft.Data.SqlClient;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Infrastructure.Data;
using FSI.CloudShopping.Infrastructure.Security;

/// <summary>
/// Repository for Authentication entity using stored procedures.
/// Implements IAuthenticationRepository following DDD + Clean Architecture.
/// </summary>
public class AuthenticationRepository : BaseRepository<Authentication, int>, IAuthenticationRepository
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

    protected override void SetInsertParameters(SqlCommand cmd, Authentication entity)
    {
        cmd.Parameters.AddWithValue("@Email", entity.Email);
        cmd.Parameters.AddWithValue("@AuthorizedAccess", entity.AuthorizedAccess);
    }

    public override async Task AddAsync(Authentication entity, CancellationToken cancellationToken = default)
    {
        using var cmd = await Connector.CreateProcedureCommandAsync(ProcInsert);
        SetInsertParameters(cmd, entity);
        var result = await cmd.ExecuteScalarAsync(cancellationToken);
        // Id is set via stored procedure return value - reflective update via protected setter
        var generatedId = Convert.ToInt32(result);
        typeof(Authentication)
            .GetProperty(nameof(Authentication.Id))!
            .SetValue(entity, generatedId);
    }

    public override Task UpdateAsync(Authentication entity, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public async Task<int> InsertAsync(string email, bool isAuthorized, CancellationToken cancellationToken = default)
    {
        using var cmd = await Connector.CreateProcedureCommandAsync(ProcInsert);
        cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = email;
        cmd.Parameters.Add("@AuthorizedAccess", SqlDbType.Bit).Value = isAuthorized;
        var result = await cmd.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt32(result);
    }

    public async Task<bool> GetAccessAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        using var cmd = await Connector.CreateProcedureCommandAsync("QueryAuthentication");
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "SELECT [PasswordHash] FROM [dbo].[Customers] WHERE [Email] = @Email AND [IsActive] = 1";
        cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = email;
        using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
        {
            var storedHash = reader["PasswordHash"].ToString();
            return _passwordHasher.VerifyPassword(password, storedHash ?? string.Empty);
        }
        return false;
    }

    public async Task<Authentication?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        using var cmd = await Connector.CreateProcedureCommandAsync("QueryAuthentication");
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "SELECT * FROM [dbo].[Authentication] WHERE [Email] = @Email";
        cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = email;
        using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? MapToEntity(reader) : null;
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
