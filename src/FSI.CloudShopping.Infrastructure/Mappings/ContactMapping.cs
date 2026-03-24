namespace FSI.CloudShopping.Infrastructure.Mappings;

using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using Microsoft.Data.SqlClient;

/// <summary>
/// Maps SqlDataReader rows to Contact domain entities.
/// Updated to use Guid IDs matching DDD Entity&lt;Guid&gt; base.
/// </summary>
public static class ContactMapping
{
    public static Contact ToContactEntity(this SqlDataReader reader)
    {
        var id = reader.GetGuid(reader.GetOrdinal("Id"));
        var customerId = reader.GetGuid(reader.GetOrdinal("CustomerId"));
        var name = reader["Name"].ToString() ?? string.Empty;
        var emailAddress = reader["Email"].ToString() ?? string.Empty;
        var phoneNumber = reader.IsDBNull(reader.GetOrdinal("Phone"))
            ? null
            : reader["Phone"].ToString();
        var position = reader.IsDBNull(reader.GetOrdinal("Position"))
            ? null
            : reader["Position"].ToString();
        var isPrimary = !reader.IsDBNull(reader.GetOrdinal("IsPrimary"))
            && reader.GetBoolean(reader.GetOrdinal("IsPrimary"));

        return new Contact(
            id: id,
            customerId: customerId,
            name: name,
            email: new Email(emailAddress),
            phone: phoneNumber is not null ? new Phone(phoneNumber) : null,
            position: position,
            isPrimary: isPrimary);
    }
}
