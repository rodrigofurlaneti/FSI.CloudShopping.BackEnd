using System.Data;
using Microsoft.Data.SqlClient;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;

namespace FSI.CloudShopping.Infrastructure.Mappings
{
    public static class CustomerMapping
    {
        public static Customer ToCustomerEntity(this SqlDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var sessionToken = reader.GetGuid(reader.GetOrdinal("SessionToken"));
            var emailStr = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null;
            var typeCode = reader["CustomerTypeCode"]?.ToString() ?? "Guest";
            var isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
            var customer = new Customer(sessionToken);
            var type = typeof(Customer);
            type.GetProperty("Id")?.SetValue(customer, id);
            type.GetProperty("IsActive")?.SetValue(customer, isActive);
            type.GetProperty("CustomerType")?.SetValue(customer, CustomerType.FromString(typeCode));
            if (!string.IsNullOrEmpty(emailStr))
                type.GetProperty("Email")?.SetValue(customer, new Email(emailStr));
            if (reader["TaxId"] != DBNull.Value)
            {
                var individual = new Individual(
                    id,
                    new TaxId(reader["TaxId"].ToString()!),
                    new PersonName(reader["FullName"].ToString()!),
                    reader["BirthDate"] as DateTime?
                );
                type.GetProperty("Individual")?.SetValue(customer, individual);
            }
            else if (reader["BusinessTaxId"] != DBNull.Value)
            {
                var company = new Company(
                    id,
                    new BusinessTaxId(reader["BusinessTaxId"].ToString()!),
                    reader["CompanyName"].ToString()!,
                    reader["StateTaxId"]?.ToString()
                );
                type.GetProperty("Company")?.SetValue(customer, company);
            }
            return customer;
        }
    }
}