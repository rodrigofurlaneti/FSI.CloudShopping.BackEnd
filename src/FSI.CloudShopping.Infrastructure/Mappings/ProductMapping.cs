using System.Data;
using Microsoft.Data.SqlClient;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Infrastructure.Mappings
{
    public static class ProductMapping
    {
        public static Product ToProductEntity(this SqlDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var skuCode = reader["Sku"].ToString() ?? string.Empty;
            var name = reader["Name"].ToString() ?? string.Empty;
            var description = reader["Description"].ToString() ?? string.Empty;
            var priceValue = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0m;
            int stockOrdinal = reader.GetOrdinal("StockQuantity");
            var stockValue = reader.IsDBNull(stockOrdinal) ? 0 : reader.GetInt32(stockOrdinal);
            var product = new Product(
                new SKU(skuCode),
                name,
                description,
                new Money(priceValue),
                new Quantity(stockValue)
            );
            var imageUrlOrdinal = reader.GetOrdinal("ImageUrl");
            if (!reader.IsDBNull(imageUrlOrdinal))
            {
                var imageUrl = reader.GetString(imageUrlOrdinal);
                product.AddImages(new List<string> { imageUrl });
            }
            var idProperty = typeof(Product).GetProperty("Id");
            idProperty?.SetValue(product, id);
            return product;
        }
    }
}