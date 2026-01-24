using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Infrastructure.Data;
using FSI.CloudShopping.Infrastructure.Mappings;
using Microsoft.Data.SqlClient;
using System.Data;
namespace FSI.CloudShopping.Infrastructure.Repositories
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(SqlDbConnector connector) : base(connector) { }
        protected override string ProcInsert => "Procedure_Cart_Upsert"; 
        protected override string ProcUpdate => "Procedure_Cart_Update";
        protected override string ProcDelete => "Procedure_Cart_Delete";
        protected override string ProcGetById => "Procedure_Cart_GetById";
        protected override string ProcGetAll => "Procedure_Cart_GetAll";
        protected string ProcGetByCustomerId => "Procedure_Cart_GetByCustomerId";
        protected string ProcGetBySessionToken => "Procedure_Cart_GetBySessionToken";
        protected string ProcGetByEmail => "Procedure_Cart_GetByEmail";
        protected string ProcCartItemsClear => "Procedure_CartItems_Clear";
        protected string ProcCartItemInsert => "Procedure_CartItem_Insert";

        public async Task<Cart?> GetByCustomerIdAsync(int customerId)
        {
            return await GetCartComplex(ProcGetByCustomerId, "@CustomerId", customerId);
        }
        public async Task<Cart?> GetBySessionTokenAsync(Guid token)
        {
            return await GetCartComplex(ProcGetBySessionToken, "@Token", token);
        }
        public async Task<Cart?> GetByEmailAsync(Email email)
        {
            return await GetCartComplex(ProcGetByEmail, "@Email", email.Address);
        }
        public async Task UpdateItemsAsync(Cart cart)
        {
            using var cmdClear = await Connector.CreateProcedureCommandAsync(ProcCartItemsClear);
            cmdClear.Parameters.AddWithValue("@CartId", cart.Id);
            await cmdClear.ExecuteNonQueryAsync();
            foreach (var item in cart.Items)
            {
                using var cmdItem = await Connector.CreateProcedureCommandAsync(ProcCartItemInsert);
                cmdItem.Parameters.AddWithValue("@CartId", cart.Id);
                cmdItem.Parameters.AddWithValue("@ProductId", item.ProductId);
                cmdItem.Parameters.AddWithValue("@Quantity", item.Quantity.Value);
                cmdItem.Parameters.AddWithValue("@UnitPrice", item.UnitPrice.Value);
                await cmdItem.ExecuteNonQueryAsync();
            }
        }
        public override async Task AddAsync(Cart entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcInsert);
            cmd.Parameters.AddWithValue("@CustomerId", entity.CustomerId);
            cmd.Parameters.AddWithValue("@UpdatedAt", entity.UpdatedAt);
            var result = await cmd.ExecuteScalarAsync();
            if (result != null)
            {
                typeof(Cart).GetProperty("Id")?.SetValue(entity, Convert.ToInt32(result));
                await UpdateItemsAsync(entity);
            }
        }
        public override async Task UpdateAsync(Cart entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcUpdate);
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@UpdatedAt", entity.UpdatedAt);
            await cmd.ExecuteNonQueryAsync();
            await UpdateItemsAsync(entity);
        }
        private async Task<Cart?> GetCartComplex(string proc, string paramName, object value)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(proc);
            cmd.Parameters.AddWithValue(paramName, value);
            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;
            var cart = reader.ToCartEntity();
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync()) cart.AddCartItem(reader);
            }
            return cart;
        }

        protected override Cart MapToEntity(SqlDataReader reader) => reader.ToCartEntity();
    }
}