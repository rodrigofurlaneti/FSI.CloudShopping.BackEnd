using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Infrastructure.Data;
using FSI.CloudShopping.Infrastructure.Mappings;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace FSI.CloudShopping.Infrastructure.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(SqlDbConnector connector) : base(connector) { }

        // Procedimentos armazenados
        protected override string ProcInsert => "Procedure_Customer_Insert";
        protected override string ProcUpdate => "Procedure_Customer_Update";
        protected override string ProcDelete => "Procedure_Customer_Delete";
        protected override string ProcGetById => "Procedure_Customer_GetById";
        protected override string ProcGetAll => "Procedure_Customer_Get";
        protected string ProcGetByEmail => "Procedure_Customer_GetByEmail";
        protected string ProcGetByCpf => "Procedure_Customer_GetByCpf";
        protected string ProcGetByCnpj => "Procedure_Customer_GetByCnpj";

        // ✅ Implementa AddAsync retornando Task<int> e usando SetId
        public override async Task<int> AddAsync(Customer entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcInsert);
            SetInsertParameters(cmd, entity);

            var result = await cmd.ExecuteScalarAsync();
            int generatedId = Convert.ToInt32(result);
            entity.Id = generatedId;
            return generatedId;
        }

        public override async Task UpdateAsync(Customer entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcUpdate);
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            SetInsertParameters(cmd, entity); // Reutiliza para parâmetros básicos
            AddIndividualParameters(cmd, entity.Individual);
            AddCompanyParameters(cmd, entity.Company);

            await cmd.ExecuteNonQueryAsync();
        }

        // ✅ Implementa SetInsertParameters obrigatório pelo BaseRepository
        protected override void SetInsertParameters(SqlCommand cmd, Customer entity)
        {
            cmd.Parameters.AddWithValue("@Email", entity.Email?.Address ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@PasswordHash", entity.Password?.Hash ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@SessionToken", entity.SessionToken);
            cmd.Parameters.AddWithValue("@CustomerTypeCode", entity.CustomerType.Code);
            cmd.Parameters.AddWithValue("@IsActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@Latitude", entity.GeoLocation?.Latitude ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Longitude", entity.GeoLocation?.Longitude ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@UserAgent", entity.DeviceInfo?.UserAgent ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Platform", entity.DeviceInfo?.Platform ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Language", entity.DeviceInfo?.Language ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@TimeZone", entity.DeviceInfo?.TimeZone ?? (object)DBNull.Value);
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
