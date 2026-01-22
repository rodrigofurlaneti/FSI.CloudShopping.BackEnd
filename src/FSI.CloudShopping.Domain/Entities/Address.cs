using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Entities
{
    public class Address : Entity
    {
        public int CustomerId { get; private set; }
        public AddressType Type { get; private set; }
        public AddressInfo Info { get; private set; }
        public bool IsDefault { get; private set; }

        protected Address() { }

        public Address(int customerId, AddressType type, AddressInfo info, bool isDefault)
        {
            CustomerId = customerId;
            Type = type;
            Info = info;
            IsDefault = isDefault;
        }

        public void SetNonDefault() => IsDefault = false;
    }
}
