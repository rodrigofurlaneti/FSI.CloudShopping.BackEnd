using FSI.CloudShopping.Domain.Core;

namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record Address
    {
        public string Street { get; init; }
        public string Number { get; init; }
        public string Neighborhood { get; init; }
        public string ZipCode { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public bool IsDefault { get; init; }

        public Address(string street, string number, string neighborhood, string zipCode, string city, string state, bool isDefault)
        {
            if (string.IsNullOrWhiteSpace(street)) throw new DomainException("Street is required.");
            if (string.IsNullOrWhiteSpace(number)) throw new DomainException("Number is required.");
            if (string.IsNullOrWhiteSpace(neighborhood)) throw new DomainException("Neighborhood is required.");
            if (string.IsNullOrWhiteSpace(zipCode)) throw new DomainException("ZipCode is required.");
            if (string.IsNullOrWhiteSpace(city)) throw new DomainException("City is required.");
            if (string.IsNullOrWhiteSpace(state)) throw new DomainException("State is required.");

            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;
            IsDefault = isDefault;
        }

        public Address SetNonDefault() => this with { IsDefault = false };
    }
}