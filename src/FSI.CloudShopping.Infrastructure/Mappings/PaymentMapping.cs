using System.Data;
using Microsoft.Data.SqlClient;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using System.Reflection;
namespace FSI.CloudShopping.Infrastructure.Mappings
{
    public static class PaymentMapping
    {
        public static Payment ToPaymentEntity(this SqlDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var orderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
            var methodStr = reader["Method"].ToString() ?? string.Empty;
            var statusStr = reader["Status"].ToString() ?? "Pending";
            var amountValue = Convert.ToDecimal(reader["Amount"]);
            var method = PaymentMethod.FromString(methodStr);
            var status = PaymentStatus.FromString(statusStr);
            var amount = new Money(amountValue);
            var payment = new Payment(orderId, method, amount);
            var type = typeof(Payment);
            type.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                ?.SetValue(payment, id);
            type.GetProperty("Status")?.SetValue(payment, status);
            return payment;
        }
    }
}