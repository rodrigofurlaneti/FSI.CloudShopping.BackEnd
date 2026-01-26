using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;

namespace FSI.CloudShopping.Domain.Entities
{
    public class Address : Entity
    {
        public int CustomerId { get; private set; }
        public AddressType AddressType { get; private set; }
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string? Neighborhood { get; private set; } // Opcional no banco, mas deve ser mapeável
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }
        public bool IsDefault { get; private set; }

        // Construtor para ORMs/Mappers
        protected Address() { }

        // Construtor principal com todas as propriedades
        public Address(
            int customerId,
            AddressType addressType,
            string street,
            string number,
            string city,
            string state,
            string zipCode,
            string? neighborhood = null,
            bool isDefault = false)
        {
            Validate(customerId, addressType, street, number, city, state, zipCode);

            CustomerId = customerId;
            AddressType = addressType;
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            ZipCode = zipCode;
            IsDefault = isDefault;
        }

        public static Address CreateDefault(
            int customerId,
            AddressType addressType,
            string street,
            string number,
            string city,
            string state,
            string zipCode,
            string? neighborhood = null)
        {
            return new Address(
                customerId,
                addressType,
                street,
                number,
                city,
                state,
                zipCode,
                neighborhood,
                isDefault: true);
        }

        public void SetAsDefault() => IsDefault = true;
        public void SetNonDefault() => IsDefault = false;

        public void UpdateNeighborhood(string? neighborhood)
        {
            Neighborhood = neighborhood;
        }

        private void Validate(int customerId, AddressType addressType, string street, string number, string city, string state, string zipCode)
        {
            if (customerId <= 0)
                throw new DomainException("CustomerId inválido.");

            if (addressType == null)
                throw new DomainException("AddressType é obrigatório.");

            if (string.IsNullOrWhiteSpace(street))
                throw new DomainException("Street é obrigatório.");

            if (string.IsNullOrWhiteSpace(number))
                throw new DomainException("Number é obrigatório.");

            if (string.IsNullOrWhiteSpace(city))
                throw new DomainException("City é obrigatório.");

            if (string.IsNullOrWhiteSpace(state))
                throw new DomainException("State é obrigatório.");

            if (string.IsNullOrWhiteSpace(zipCode))
                throw new DomainException("ZipCode é obrigatório.");
        }
    }
}