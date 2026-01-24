using FSI.CloudShopping.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.CloudShopping.Domain.Entities
{
    public class Address : Entity
    {
        public int CustomerId { get; private set; }
        public string AddressType { get; private set; }
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string? Neighborhood { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }
        public bool IsDefault { get; private set; }

        protected Address() { }

        public Address(int customerId, string addressType, string street, string number,
                       string city, string state, string zipCode, bool isDefault = false)
        {
            CustomerId = customerId;
            AddressType = addressType;
            Street = street;
            Number = number;
            City = city;
            State = state;
            ZipCode = zipCode;
            IsDefault = isDefault;
        }

        // Comportamento para mudar o status de padrão
        public void SetAsDefault() => IsDefault = true;
        public void SetNonDefault() => IsDefault = false;
    }
}