using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Infrastructure.Data;
using FSI.CloudShopping.Infrastructure.Mappings;
using Microsoft.Data.SqlClient;
using System.Data;
namespace FSI.CloudShopping.Infrastructure.Repositories
{
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        public AddressRepository(SqlDbConnector connector) : base(connector) { }

        protected override string ProcInsert => "Procedure_Address_Insert";
        protected override string ProcUpdate => "Procedure_Address_Update";
        protected override string ProcDelete => "Procedure_Address_Delete";
        protected override string ProcGetById => "Procedure_Address_GetById";
        protected override string ProcGetAll => "Procedure_Address_GetAll";
        protected string ProcGetByCustomer => "Procedure_Address_GetByCustomerId";
        protected string ProcGetDefault => "Procedure_Address_GetDefaultByCustomerId";

        public override async Task AddAsync(Address entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcInsert);
            AddCommonParameters(cmd, entity);
            await cmd.ExecuteNonQueryAsync();
        }
        public override async Task UpdateAsync(Address entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcUpdate);
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            AddCommonParameters(cmd, entity);
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<IEnumerable<Address>> GetByCustomerIdAsync(int customerId)
        {
            var addresses = new List<Address>();
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetByCustomer);
            cmd.Parameters.AddWithValue("@CustomerId", customerId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                addresses.Add(MapToEntity(reader));

            return addresses;
        }
        public async Task<Address?> GetDefaultAddressAsync(int customerId)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetDefault);
            cmd.Parameters.AddWithValue("@CustomerId", customerId);

            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapToEntity(reader) : null;
        }
        private void AddCommonParameters(SqlCommand cmd, Address entity)
        {
            cmd.Parameters.AddWithValue("@CustomerId", entity.CustomerId);
            cmd.Parameters.AddWithValue("@AddressType", entity.AddressType);
            cmd.Parameters.AddWithValue("@Street", entity.Street);
            cmd.Parameters.AddWithValue("@Number", entity.Number);
            cmd.Parameters.AddWithValue("@Neighborhood", entity.Neighborhood ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@City", entity.City);
            cmd.Parameters.AddWithValue("@State", entity.State);
            cmd.Parameters.AddWithValue("@ZipCode", entity.ZipCode);
            cmd.Parameters.AddWithValue("@IsDefault", entity.IsDefault);
        }
        protected override Address MapToEntity(SqlDataReader reader) => reader.ToAddressEntity();
    }
}