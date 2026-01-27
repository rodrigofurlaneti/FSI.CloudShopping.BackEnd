using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Infrastructure.Data;
using FSI.CloudShopping.Infrastructure.Mappings;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSI.CloudShopping.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(SqlDbConnector connector) : base(connector) { }

        protected override string ProcInsert => "Procedure_Order_Insert";
        protected override string ProcUpdate => "Procedure_Order_UpdateStatus";
        protected override string ProcDelete => "Procedure_Order_Delete";
        protected override string ProcGetById => "Procedure_Order_GetById";
        protected override string ProcGetAll => "Procedure_Order_GetAll";

        public override async Task<int> AddAsync(Order entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcInsert);
            SetInsertParameters(cmd, entity);

            var orderId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            entity.Id = orderId; 
            foreach (var item in entity.Items)
            {
                using var cmdItem = await Connector.CreateProcedureCommandAsync("Procedure_OrderItem_Insert");
                cmdItem.Parameters.AddWithValue("@OrderId", orderId);
                cmdItem.Parameters.AddWithValue("@ProductId", item.ProductId);
                cmdItem.Parameters.AddWithValue("@Quantity", item.Quantity.Value);
                cmdItem.Parameters.AddWithValue("@UnitPrice", item.UnitPrice.Value);
                await cmdItem.ExecuteNonQueryAsync();
            }

            return orderId;
        }

        public override async Task UpdateAsync(Order entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcUpdate);
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@Status", entity.Status.Code);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Order?> GetOrderWithItemsAsync(int orderId)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync("Procedure_Order_GetWithItems");
            cmd.Parameters.AddWithValue("@OrderId", orderId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            var order = reader.ToOrderEntity();
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                    order.AddItem(reader);
            }

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            var orders = new List<Order>();
            using var cmd = await Connector.CreateProcedureCommandAsync("Procedure_Order_GetByCustomerId");
            cmd.Parameters.AddWithValue("@CustomerId", customerId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                orders.Add(MapToEntity(reader));

            return orders;
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            var orders = new List<Order>();
            using var cmd = await Connector.CreateProcedureCommandAsync("Procedure_Order_GetByStatus");
            cmd.Parameters.AddWithValue("@Status", status.Code);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                orders.Add(MapToEntity(reader));

            return orders;
        }

        // Padroniza parâmetros do insert
        protected override void SetInsertParameters(SqlCommand cmd, Order entity)
        {
            cmd.Parameters.AddWithValue("@CustomerId", entity.CustomerId);
            cmd.Parameters.AddWithValue("@ShippingAddressId", entity.ShippingAddressId);
            cmd.Parameters.AddWithValue("@TotalAmount", entity.TotalAmount.Value);
            cmd.Parameters.AddWithValue("@Status", entity.Status.Code);
        }

        protected override Order MapToEntity(SqlDataReader reader) => reader.ToOrderEntity();
    }
}
