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
    public static class ContactMapping
    {
        public static Contact ToContactEntity(this SqlDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var customerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
            var fullName = reader["Name"].ToString() ?? string.Empty;
            var emailAddress = reader["Email"].ToString() ?? string.Empty;
            var phoneNumber = reader["Phone"].ToString() ?? string.Empty;
            var position = reader["Position"].ToString() ?? string.Empty;
            var contact = new Contact(
                customerId,
                new PersonName(fullName),
                new Email(emailAddress),
                new Phone(phoneNumber),
                position
            );
            typeof(Contact).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                ?.SetValue(contact, id);
            return contact;
        }
    }
}