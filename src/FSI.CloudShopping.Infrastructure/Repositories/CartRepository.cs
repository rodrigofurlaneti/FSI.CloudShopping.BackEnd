using System.Data;
using Microsoft.Data.SqlClient;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Infrastructure.Data;
using FSI.CloudShopping.Infrastructure.Mappings;

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

        public async Task<Cart?> GetByCustomerIdAsync(int customerId) =>
            await GetCartComplex(ProcGetByCustomerId, "@CustomerId", customerId);

        public async Task<Cart?> GetBySessionTokenAsync(Guid token) =>
            await GetCartComplex(ProcGetBySessionToken, "@Token", token);

        public async Task<Cart?> GetByEmailAsync(Email email) =>
            await GetCartComplex(ProcGetByEmail, "@Email", email.Address);

        public async Task UpdateItemsAsync(Cart cart)
        {
            if (cart.Items == null || !cart.Items.Any()) return;

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

        public override async Task<int> AddAsync(Cart entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcInsert);
            SetInsertParameters(cmd, entity);
            var cartId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            entity.Id = cartId;
            if (entity.Items.Any())
                await UpdateItemsAsync(entity);
            return cartId;
        }

        public override async Task UpdateAsync(Cart entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcUpdate);
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@UpdatedAt", entity.UpdatedAt);
            await cmd.ExecuteNonQueryAsync();

            if (entity.Items.Any())
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
                while (await reader.ReadAsync())
                    cart.AddCartItem(reader);
            }

            return cart;
        }

        protected override void SetInsertParameters(SqlCommand cmd, Cart entity)
        {
            cmd.Parameters.AddWithValue("@CustomerId", entity.CustomerId);
            cmd.Parameters.AddWithValue("@UpdatedAt", entity.UpdatedAt);
        }

        protected override Cart MapToEntity(SqlDataReader reader) => reader.ToCartEntity();
    }
}
