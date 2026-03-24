namespace FSI.CloudShopping.Infrastructure.Repositories;

using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Infrastructure.Data;
using FSI.CloudShopping.Infrastructure.Mappings;
using Microsoft.Data.SqlClient;

/// <summary>
/// Repository for Contact entity using stored procedures.
/// Migrated to BaseRepository&lt;Contact, Guid&gt; to align with DDD generic entity base.
/// </summary>
public class ContactRepository : BaseRepository<Contact, Guid>, IContactRepository
{
    public ContactRepository(SqlDbConnector connector) : base(connector) { }

    protected override string ProcInsert => "Procedure_Contact_Insert";
    protected override string ProcUpdate => "Procedure_Contact_Update";
    protected override string ProcDelete => "Procedure_Contact_Delete";
    protected override string ProcGetById => "Procedure_Contact_GetById";
    protected override string ProcGetAll => "Procedure_Contact_GetAll";
    protected string ProcGetByCustomer => "Procedure_Contact_GetByCustomerId";

    public override async Task AddAsync(Contact entity, CancellationToken cancellationToken = default)
    {
        using var cmd = await Connector.CreateProcedureCommandAsync(ProcInsert);
        SetInsertParameters(cmd, entity);
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public override async Task UpdateAsync(Contact entity, CancellationToken cancellationToken = default)
    {
        using var cmd = await Connector.CreateProcedureCommandAsync(ProcUpdate);
        cmd.Parameters.AddWithValue("@Id", entity.Id);
        SetInsertParameters(cmd, entity);
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<IEnumerable<Contact>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var contacts = new List<Contact>();
        using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetByCustomer);
        cmd.Parameters.AddWithValue("@CustomerId", customerId);
        using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
            contacts.Add(MapToEntity(reader));
        return contacts;
    }

    public async Task<Contact?> GetPrimaryContactAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var contacts = await GetByCustomerIdAsync(customerId, cancellationToken);
        return contacts.FirstOrDefault(c => c.IsPrimary);
    }

    protected override void SetInsertParameters(SqlCommand cmd, Contact entity)
    {
        cmd.Parameters.AddWithValue("@CustomerId", entity.CustomerId);
        cmd.Parameters.AddWithValue("@Name", entity.Name);
        cmd.Parameters.AddWithValue("@Email", entity.Email.Value);
        cmd.Parameters.AddWithValue("@Phone", (object?)entity.Phone?.Value ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Position", (object?)entity.Position ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@IsPrimary", entity.IsPrimary);
    }

    protected override Contact MapToEntity(SqlDataReader reader) => reader.ToContactEntity();
}
