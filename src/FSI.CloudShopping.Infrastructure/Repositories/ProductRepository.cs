using System.Data;
using Microsoft.Data.SqlClient;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Infrastructure.Data;
using FSI.CloudShopping.Domain.Core;

namespace FSI.CloudShopping.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SqlDbConnector _connector;

        public ProductRepository(string connectionString) => _connector = new SqlDbConnector(connectionString);

        public async Task AddAsync(Product entity)
        {
            using var cmd = await _connector.CreateProcedureCommandAsync("Procedure_Product_Insert");
            MapEntityToParameters(entity, cmd.Parameters);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            using var cmd = await _connector.CreateProcedureCommandAsync("Procedure_Product_Update");
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            MapEntityToParameters(entity, cmd.Parameters);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task RemoveAsync(int id)
        {
            using var cmd = await _connector.CreateProcedureCommandAsync("Procedure_Product_Delete");
            cmd.Parameters.AddWithValue("@Id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using var cmd = await _connector.CreateProcedureCommandAsync("Procedure_Product_GetById");
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            return await MapSingleProduct(reader);
        }

        public async Task<Product?> GetBySkuAsync(SKU sku)
        {
            using var cmd = await _connector.CreateProcedureCommandAsync("Procedure_Product_GetBySku");
            cmd.Parameters.AddWithValue("@Sku", sku.Code);
            using var reader = await cmd.ExecuteReaderAsync();
            return await MapSingleProduct(reader);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var cmd = await _connector.CreateProcedureCommandAsync("Procedure_Product_GetAll");
            using var reader = await cmd.ExecuteReaderAsync();
            return await MapCollection(reader);
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            using var cmd = await _connector.CreateProcedureCommandAsync("Procedure_Product_GetByCategoryId");
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);
            using var reader = await cmd.ExecuteReaderAsync();
            return await MapCollection(reader);
        }

        private async Task<Product?> MapSingleProduct(SqlDataReader reader)
        {
            if (!await reader.ReadAsync()) return null;
            return LoadFromReader(reader);
        }

        private async Task<IEnumerable<Product>> MapCollection(SqlDataReader reader)
        {
            var products = new List<Product>();
            while (await reader.ReadAsync())
            {
                products.Add(LoadFromReader(reader));
            }
            return products;
        }

        private Product LoadFromReader(SqlDataReader reader)
        {
            var product = new Product(
                new SKU(reader["Sku"].ToString()!),
                reader["Name"].ToString()!,
                new Money(Convert.ToDecimal(reader["Price"])),
                new Quantity(Convert.ToInt32(reader["Stock"]))
            );
            var idProperty = typeof(Entity).GetProperty("Id");
            idProperty?.SetValue(product, Convert.ToInt32(reader["Id"]));

            return product;
        }

        private void MapEntityToParameters(Product p, SqlParameterCollection col)
        {
            col.AddWithValue("@Sku", p.Sku.Code);
            col.AddWithValue("@Name", p.Name);
            col.AddWithValue("@Price", p.Price.Value);
            col.AddWithValue("@Stock", p.Stock.Value);
        }

        public async Task<int> SaveChangesAsync() => await Task.FromResult(1);

        public void Dispose() => _connector.Dispose();
    }
}