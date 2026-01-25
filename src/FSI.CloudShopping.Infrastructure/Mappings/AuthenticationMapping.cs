using FSI.CloudShopping.Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FSI.CloudShopping.Infrastructure.Mappings
{
    public static class AuthenticationMapping
    {
        public static Authentication ToAuthenticationEntity(this SqlDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var email = reader["Email"].ToString() ?? string.Empty;
            var authorizedAccess = reader.GetBoolean(reader.GetOrdinal("AuthorizedAccess"));
            var createdAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
            return new Authentication(id, email, authorizedAccess, createdAt);
        }
    }
}