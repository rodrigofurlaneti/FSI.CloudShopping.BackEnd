using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace FSI.CloudShopping.Infrastructure.Mappings
{
    public static class CartMapping
    {
        public static Cart ToCartEntity(this SqlDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var customerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
            var updatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"));
            var cart = (Cart)Activator.CreateInstance(typeof(Cart), true)!;
            var type = typeof(Cart);
            type.GetProperty("Id")?.SetValue(cart, id);
            type.GetProperty("CustomerId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.SetValue(cart, customerId);
            type.GetProperty("UpdatedAt", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.SetValue(cart, updatedAt);

            return cart;
        }

        public static void AddCartItem(this Cart cart, SqlDataReader reader)
        {
            var productId = reader.GetInt32(reader.GetOrdinal("ProductId"));
            var quantityValue = reader.GetInt32(reader.GetOrdinal("Quantity"));
            var priceValue = Convert.ToDecimal(reader["UnitPrice"]);
            var productName = reader.GetString(reader.GetOrdinal("Name"));
            var imageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl"))
                ? null
                : reader.GetString(reader.GetOrdinal("ImageUrl"));
            var item = new CartItem(
                productId,
                new Quantity(quantityValue),
                new Money(priceValue),
                productName,
                imageUrl
            );
            var itemsField = typeof(Cart).GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic);
            var itemsList = (List<CartItem>)itemsField?.GetValue(cart)!;
            itemsList.Add(item);
        }
    }
}