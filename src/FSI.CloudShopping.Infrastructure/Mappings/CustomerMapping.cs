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
            var customer = (Customer)Activator.CreateInstance(
                typeof(Customer),
                nonPublic: true
            )!;

            Set(customer, "Id", reader.GetInt32(reader.GetOrdinal("Id")));
            Set(customer, "SessionToken", reader.GetGuid(reader.GetOrdinal("SessionToken")));
            Set(customer, "IsActive", reader.GetBoolean(reader.GetOrdinal("IsActive")));
            Set(customer, "CustomerType",
                CustomerType.FromString(reader["CustomerType"]?.ToString())
            );

            // 🌍 Geolocalização 
            if (reader["Latitude"] != DBNull.Value && reader["Longitude"] != DBNull.Value)
            {
                var geo = new GeoLocation(
                    reader.GetDecimal(reader.GetOrdinal("Latitude")),
                    reader.GetDecimal(reader.GetOrdinal("Longitude"))
                );

                Set(customer, "GeoLocation", geo);
            }

            if (reader["UserAgent"] != DBNull.Value)
            {
                var deviceInfo = new DeviceInfo(
                    reader["UserAgent"].ToString(),
                    reader["Platform"]?.ToString(),
                    reader["Language"]?.ToString(),
                    reader["TimeZone"]?.ToString()
                );

                Set(customer, "DeviceInfo", deviceInfo);
            }

            // Credenciais
            if (reader["Email"] != DBNull.Value)
                Set(customer, "Email", new Email(reader["Email"].ToString()!));

            if (reader["PasswordHash"] != DBNull.Value)
                Set(customer, "Password", new Password(reader["PasswordHash"].ToString()!));

            // Pessoa Física (B2C)
            if (reader["TaxId"] != DBNull.Value)
            {
                var individual = new Individual(
                    customer.Id,
                    new TaxId(reader["TaxId"].ToString()!),
                    new PersonName(reader["FullName"].ToString()!),
                    reader["BirthDate"] as DateTime?
                );

                Set(customer, "Individual", individual);
            }
            // Pessoa Jurídica (B2B)
            else if (reader["BusinessTaxId"] != DBNull.Value)
            {
                var company = new Company(
                    customer.Id,
                    new BusinessTaxId(reader["BusinessTaxId"].ToString()!),
                    reader["CompanyName"].ToString()!,
                    reader["StateTaxId"]?.ToString()
                );

                Set(customer, "Company", company);
            }

            return customer;
        }


        private static void Set(object target, string property, object? value)
        {
            target
                .GetType()
                .GetProperty(property)?
                .SetValue(target, value);
        }
    }
}
