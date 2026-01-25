using FSI.CloudShopping.Domain.Core;

namespace FSI.CloudShopping.Domain.Entities
{
    public class Authentication : Entity
    {
        public string Email { get; set; }
        public bool AuthorizedAccess { get; set; }
        public Authentication() { }
        public Authentication(int id, string email, bool authorizedAccess, DateTime createdAt)
        {
            Id = id;
            Email = email;
            AuthorizedAccess = authorizedAccess;
        }
    }
}