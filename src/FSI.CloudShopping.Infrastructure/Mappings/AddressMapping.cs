using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Reflection;

namespace FSI.CloudShopping.Infrastructure.Mappings
{
    public static class AddressMapping
    {
        public static Address ToAddressEntity(this SqlDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var customerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
            var street = reader["Street"]?.ToString() ?? string.Empty;
            var number = reader["Number"]?.ToString() ?? string.Empty;
            var city = reader["City"]?.ToString() ?? string.Empty;
            var state = reader["State"]?.ToString() ?? string.Empty;
            var zipCode = reader["ZipCode"]?.ToString() ?? string.Empty;
            var isDefault = reader.GetBoolean(reader.GetOrdinal("IsDefault"));
            var neighborhood = reader["Neighborhood"] != DBNull.Value
                ? reader["Neighborhood"].ToString()
                : null;
            var addressTypeString = reader["AddressType"]?.ToString() ?? "Shipping";
            var addressType = AddressType.FromString(addressTypeString);
            var address = new Address(
                customerId,
                addressType,
                street,
                number,
                city,
                state,
                zipCode,
                neighborhood,
                isDefault
            );
            var propertyInfo = typeof(Address).GetProperty("Id")
                               ?? typeof(Address).BaseType?.GetProperty("Id");
            propertyInfo?.SetValue(address, id);
            return address;
        }
    }
}