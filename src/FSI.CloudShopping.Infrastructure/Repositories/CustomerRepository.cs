using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Core;

namespace FSI.CloudShopping.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString) => _connectionString = connectionString;

        public async Task AddAsync(Customer entity)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = CreateCommand("Procedure_Customer_Insert", conn);
            cmd.Parameters.AddWithValue("@Email", entity.Email.Address);
            cmd.Parameters.AddWithValue("@PasswordHash", Encoding.UTF8.GetBytes(entity.Password.Hash));
            cmd.Parameters.AddWithValue("@CustomerType", entity.Document.IsCompany ? "PJ" : "PF");
            cmd.Parameters.AddWithValue("@TaxId", entity.Document.Number);
            cmd.Parameters.AddWithValue("@Name", "Nome Mock"); 

            await conn.OpenAsync();
            entity.GetType().GetProperty("Id")?.SetValue(entity, Convert.ToInt32(await cmd.ExecuteScalarAsync()));
        }

        public async Task UpdateAsync(Customer entity)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = CreateCommand("Procedure_Customer_Update", conn);
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@Email", entity.Email.Address);
            cmd.Parameters.AddWithValue("@CustomerType", entity.Document.IsCompany ? "PJ" : "PF");
            cmd.Parameters.AddWithValue("@TaxId", entity.Document.Number);
            cmd.Parameters.AddWithValue("@IsActive", entity.IsActive);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task RemoveAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = CreateCommand("Procedure_Customer_Delete", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id) => await FetchSingle(new SqlParameter("@Id", id));
        public async Task<Customer?> GetByEmailAsync(Email email) => await FetchSingle(new SqlParameter("@Email", email.Address));
        public async Task<Customer?> GetByDocumentAsync(TaxId doc) => await FetchSingle(new SqlParameter("@TaxId", doc.Number));

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var list = new List<Customer>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = CreateCommand("Procedure_Customer_GetByFilter", conn);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync()) list.Add(Map(r));
            return list;
        }

        public async Task<int> SaveChangesAsync() => await Task.FromResult(1);

        private async Task<Customer?> FetchSingle(SqlParameter param)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = CreateCommand("Procedure_Customer_GetByFilter", conn);
            cmd.Parameters.Add(param);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            if (!await r.ReadAsync()) return null;
            return Map(r);
        }

        private Customer Map(SqlDataReader r)
        {
            var customer = new Customer(
                new Email(r["Email"].ToString()!),
                new TaxId(r["TaxNumber"].ToString()!),
                new Password(Encoding.UTF8.GetString((byte[])r["PasswordHash"]))
            );
            typeof(Entity).GetProperty("Id")?.SetValue(customer, Convert.ToInt32(r["Id"]));
            return Sync(customer, Convert.ToBoolean(r["IsActive"]));
        }

        private Customer Sync(Customer c, bool active)
        {
            if (active) c.Activate();
            if (!active) c.Deactivate();
            return c;
        }

        private SqlCommand CreateCommand(string p, SqlConnection c) => new(p, c) { CommandType = CommandType.StoredProcedure };
        public void Dispose() { }
    }
}