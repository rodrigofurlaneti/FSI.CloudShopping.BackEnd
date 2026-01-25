using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Infrastructure.Data;
using FSI.CloudShopping.Infrastructure.Mappings;
using Microsoft.Data.SqlClient;
using System.Data;
namespace FSI.CloudShopping.Infrastructure.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(SqlDbConnector connector) : base(connector) { }

        protected override string ProcInsert => "Procedure_Customer_Insert";
        protected override string ProcUpdate => "Procedure_Customer_Update";
        protected override string ProcDelete => "Procedure_Customer_Delete";
        protected override string ProcGetById => "Procedure_Customer_GetById";
        protected override string ProcGetAll => "Procedure_Customer_Get";
        protected string ProcGetByEmail => "Procedure_Customer_GetByEmail";
        protected string ProcGetByCpf => "Procedure_Customer_GetByCpf";
        protected string ProcGetByCnpj => "Procedure_Customer_GetByCnpj";
        public override async Task AddAsync(Customer entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcInsert);
            cmd.Parameters.AddWithValue("@Email", entity.Email?.Address ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@PasswordHash", entity.Password?.Hash ?? (object)DBNull.Value); // O .Hash é string
            cmd.Parameters.AddWithValue("@SessionToken", entity.SessionToken);
            cmd.Parameters.AddWithValue("@CustomerTypeCode", entity.CustomerType.Code);
            cmd.Parameters.AddWithValue("@IsActive", entity.IsActive);
            await cmd.ExecuteNonQueryAsync();
        }
        public override async Task UpdateAsync(Customer entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcUpdate);
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@Email", entity.Email?.Address ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@PasswordHash", entity.Password?.Hash ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CustomerTypeCode", entity.CustomerType.Code);
            cmd.Parameters.AddWithValue("@IsActive", entity.IsActive);
            AddIndividualParameters(cmd, entity.Individual);
            AddCompanyParameters(cmd, entity.Company);
            await cmd.ExecuteNonQueryAsync();
        }
        private void AddIndividualParameters(SqlCommand cmd, Individual? individual)
        {
            cmd.Parameters.AddWithValue("@TaxId", individual?.TaxId.Number ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@FullName", individual?.FullName.FullName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@BirthDate", individual?.BirthDate ?? (object)DBNull.Value);
        }
        private void AddCompanyParameters(SqlCommand cmd, Company? company)
        {
            cmd.Parameters.AddWithValue("@BusinessTaxId", company?.BusinessTaxId.Number ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CompanyName", company?.CompanyName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@StateTaxId", company?.StateTaxId ?? (object)DBNull.Value);
        }
        public async Task<Customer?> GetByEmailAsync(Email email)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetByEmail);
            cmd.Parameters.AddWithValue("@Email", email.Address);
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapToEntity(reader) : null;
        }
        public async Task<Customer?> GetByIndividualDocumentAsync(string cpf)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetByCpf);
            cmd.Parameters.AddWithValue("@Cpf", cpf);
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapToEntity(reader) : null;
        }
        public async Task<Customer?> GetByCompanyDocumentAsync(string cnpj)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetByCnpj);
            cmd.Parameters.AddWithValue("@Cnpj", cnpj);
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapToEntity(reader) : null;
        }
        protected override Customer MapToEntity(SqlDataReader reader) => reader.ToCustomerEntity();
    }
}