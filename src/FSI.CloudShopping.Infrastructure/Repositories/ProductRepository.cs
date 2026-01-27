using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Infrastructure.Data;
using FSI.CloudShopping.Infrastructure.Mappings;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSI.CloudShopping.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(SqlDbConnector connector) : base(connector) { }

        protected override string ProcInsert => "Procedure_Product_Insert";
        protected override string ProcUpdate => "Procedure_Product_Update";
        protected override string ProcDelete => "Procedure_Product_Delete";
        protected override string ProcGetById => "Procedure_Product_GetById";
        protected override string ProcGetAll => "Procedure_Product_Get";
        protected string ProcGetBySku => "Procedure_Product_GetBySku";
        protected string ProcGetByCategoryId => "Procedure_Product_GetByCategoryId";
        protected string ProcGetByAvailable => "Procedure_Product_GetAvailable";
        protected string ProcGetLowStock => "Procedure_Product_GetLowStock";

        public override async Task<int> AddAsync(Product entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcInsert);
            SetCommonParameters(cmd, entity);

            var result = await cmd.ExecuteScalarAsync();
            int generatedId = Convert.ToInt32(result);
            entity.Id = generatedId; 
            return generatedId;
        }

        public override async Task UpdateAsync(Product entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcUpdate);
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            SetCommonParameters(cmd, entity);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Product?> GetBySkuAsync(SKU sku)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetBySku);
            cmd.Parameters.AddWithValue("@Sku", sku.Code);

            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapToEntity(reader) : null;
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            var products = new List<Product>();
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetByCategoryId);
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                products.Add(MapToEntity(reader));

            return products;
        }

        public async Task<IEnumerable<Product>> GetAvailableProductsAsync()
        {
            var products = new List<Product>();
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetByAvailable);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                products.Add(MapToEntity(reader));

            return products;
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold)
        {
            var products = new List<Product>();
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetLowStock);
            cmd.Parameters.AddWithValue("@Threshold", threshold);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                products.Add(MapToEntity(reader));

            return products;
        }

        protected override void SetInsertParameters(SqlCommand cmd, Product entity) => SetCommonParameters(cmd, entity);

        private void SetCommonParameters(SqlCommand cmd, Product entity)
        {
            cmd.Parameters.AddWithValue("@Sku", entity.Sku.Code);
            cmd.Parameters.AddWithValue("@Name", entity.Name);
            cmd.Parameters.AddWithValue("@Description", entity.Description);
            cmd.Parameters.AddWithValue("@Price", entity.Price.Value);
            cmd.Parameters.AddWithValue("@Stock", entity.Stock.Value);
        }

        protected override Product MapToEntity(SqlDataReader reader) => reader.ToProductEntity();
    }
}
