using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Entities
{
    public class Contact : Entity
    {
        public int CustomerId { get; private set; }
        public PersonName Name { get; private set; }
        public Email Email { get; private set; }
        public Phone Phone { get; private set; }
        public string Position { get; private set; } 
        protected Contact() { }
        public Contact(int customerId, PersonName name, Email email, Phone phone, string position)
        {
            CustomerId = customerId;
            Name = name;
            Email = email;
            Phone = phone;
            Position = position;
        }
    }
}
