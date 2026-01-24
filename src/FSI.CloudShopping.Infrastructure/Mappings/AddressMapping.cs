using System.Data;
using Microsoft.Data.SqlClient;
using FSI.CloudShopping.Domain.Entities;
using System.Reflection;
namespace FSI.CloudShopping.Infrastructure.Mappings
{
    public static class AddressMapping
    {
        public static Address ToAddressEntity(this SqlDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var customerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
            var addressType = reader["AddressType"].ToString() ?? string.Empty;
            var street = reader["Street"].ToString() ?? string.Empty;
            var number = reader["Number"].ToString() ?? string.Empty;
            var neighborhood = reader["Neighborhood"] != DBNull.Value ? reader["Neighborhood"].ToString() : null;
            var city = reader["City"].ToString() ?? string.Empty;
            var state = reader["State"].ToString() ?? string.Empty;
            var zipCode = reader["ZipCode"].ToString() ?? string.Empty;
            var isDefault = reader.GetBoolean(reader.GetOrdinal("IsDefault"));
            var address = new Address(customerId, addressType, street, number, city, state, zipCode, isDefault);
            var type = typeof(Address);
            type.GetProperty("Id")?.SetValue(address, id);
            if (!string.IsNullOrEmpty(neighborhood))
                type.GetProperty("Neighborhood")?.SetValue(address, neighborhood);
            return address;
        }
    }
}