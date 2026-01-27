using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Infrastructure.Data;
using FSI.CloudShopping.Infrastructure.Mappings;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSI.CloudShopping.Infrastructure.Repositories
{
    public class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        public ContactRepository(SqlDbConnector connector) : base(connector) { }

        protected override string ProcInsert => "Procedure_Contact_Insert";
        protected override string ProcUpdate => "Procedure_Contact_Update";
        protected override string ProcDelete => "Procedure_Contact_Delete";
        protected override string ProcGetById => "Procedure_Contact_GetById";
        protected override string ProcGetAll => "Procedure_Contact_GetAll";
        protected string ProcGetByCustomer => "Procedure_Contact_GetByCustomerId";

        public override async Task<int> AddAsync(Contact entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcInsert);
            SetInsertParameters(cmd, entity);

            var result = await cmd.ExecuteScalarAsync();
            int generatedId = Convert.ToInt32(result);
            entity.Id = generatedId; 
            return generatedId;
        }

        public override async Task UpdateAsync(Contact entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcUpdate);
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            SetInsertParameters(cmd, entity);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Contact>> GetByCustomerIdAsync(int customerId)
        {
            var contacts = new List<Contact>();
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetByCustomer);
            cmd.Parameters.AddWithValue("@CustomerId", customerId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                contacts.Add(MapToEntity(reader));

            return contacts;
        }

        protected override void SetInsertParameters(SqlCommand cmd, Contact entity)
        {
            cmd.Parameters.AddWithValue("@CustomerId", entity.CustomerId);
            cmd.Parameters.AddWithValue("@Name", entity.Name.FullName);
            cmd.Parameters.AddWithValue("@Email", entity.Email.Address);
            cmd.Parameters.AddWithValue("@Phone", entity.Phone.Number);
            cmd.Parameters.AddWithValue("@Position", entity.Position);
        }

        protected override Contact MapToEntity(SqlDataReader reader) => reader.ToContactEntity();
    }
}
