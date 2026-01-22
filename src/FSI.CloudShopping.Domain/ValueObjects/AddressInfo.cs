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
            Street = street;
            Number = number;
            ZipCode = zipCode;
            City = city;
            State = state;
        }
    }
}
