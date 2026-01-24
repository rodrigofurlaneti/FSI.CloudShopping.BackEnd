using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace FSI.CloudShopping.Infrastructure.Mappings
{
    public static class OrderMapping
    {
        public static Order ToOrderEntity(this SqlDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var customerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
            var shippingAddressId = reader.GetInt32(reader.GetOrdinal("ShippingAddressId"));
            var orderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate"));
            var statusCode = reader["Status"].ToString() ?? "Pending";
            var totalAmount = Convert.ToDecimal(reader["TotalAmount"]);
            var order = (Order)Activator.CreateInstance(typeof(Order), true)!;
            var type = typeof(Order);
            type.GetProperty("Id")?.SetValue(order, id);
            type.GetProperty("CustomerId")?.SetValue(order, customerId);
            type.GetProperty("ShippingAddressId")?.SetValue(order, shippingAddressId);
            type.GetProperty("OrderDate")?.SetValue(order, orderDate);
            type.GetProperty("Status")?.SetValue(order, OrderStatus.FromCode(statusCode));
            type.GetProperty("TotalAmount")?.SetValue(order, new Money(totalAmount));
            return order;
        }
        public static void AddItem(this Order order, SqlDataReader reader)
        {
            var productId = reader.GetInt32(reader.GetOrdinal("ProductId"));
            var quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));
            var unitPrice = Convert.ToDecimal(reader["UnitPrice"]);
            var item = new OrderItem(productId, new Quantity(quantity), new Money(unitPrice));
            var itemsField = typeof(Order).GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic);
            var itemsList = (List<OrderItem>)itemsField?.GetValue(order)!;
            itemsList.Add(item);
        }
    }
}