using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Infrastructure.Data;
using FSI.CloudShopping.Infrastructure.Mappings;
using Microsoft.Data.SqlClient;
namespace FSI.CloudShopping.Infrastructure.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(SqlDbConnector connector) : base(connector) { }
        protected override string ProcInsert => "Procedure_Payment_Insert";
        protected override string ProcUpdate => "Procedure_Payment_UpdateStatus";
        protected override string ProcDelete => "Procedure_Payment_Delete";
        protected override string ProcGetById => "Procedure_Payment_GetById";
        protected override string ProcGetAll => "Procedure_Payment_GetAll";
        protected string ProcGetByOrder => "Procedure_Payment_GetByOrderId";
        protected string ProcGetHistory => "Procedure_Payment_GetHistoryByOrderId";
        public override async Task AddAsync(Payment entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcInsert);
            cmd.Parameters.AddWithValue("@OrderId", entity.OrderId);
            cmd.Parameters.AddWithValue("@Method", entity.Method.Description);
            cmd.Parameters.AddWithValue("@Amount", entity.Amount.Value);
            cmd.Parameters.AddWithValue("@Status", entity.Status.Description);
            await cmd.ExecuteNonQueryAsync();
        }
        public override async Task UpdateAsync(Payment entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcUpdate);
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@Status", entity.Status.Description);
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<Payment?> GetByOrderIdAsync(int orderId)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetByOrder);
            cmd.Parameters.AddWithValue("@OrderId", orderId);
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapToEntity(reader) : null;
        }
        public async Task<IEnumerable<Payment>> GetHistoryByOrderIdAsync(int orderId)
        {
            var payments = new List<Payment>();
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetHistory);
            cmd.Parameters.AddWithValue("@OrderId", orderId);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) payments.Add(MapToEntity(reader));
            return payments;
        }
        protected override Payment MapToEntity(SqlDataReader reader) => reader.ToPaymentEntity();
    }
}