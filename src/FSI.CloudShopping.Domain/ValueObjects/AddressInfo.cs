using FSI.CloudShopping.Domain.Core;

namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record AddressInfo
    {
        public string Street { get; }
        public string Number { get; }
        public string ZipCode { get; }
        public string City { get; }
        public string State { get; }

        public AddressInfo(string street, string number, string zipCode, string city, string state)
        {
            if (string.IsNullOrWhiteSpace(street)) throw new DomainException("Street is required.");
            if (string.IsNullOrWhiteSpace(number)) throw new DomainException("Number is required.");
            if (string.IsNullOrWhiteSpace(zipCode)) throw new DomainException("ZipCode is required.");
            if (string.IsNullOrWhiteSpace(city)) throw new DomainException("City is required.");
            if (string.IsNullOrWhiteSpace(state)) throw new DomainException("State is required.");

            Street = street;
            Number = number;
            ZipCode = zipCode;
            City = city;
            State = state;
        }
    }
}
