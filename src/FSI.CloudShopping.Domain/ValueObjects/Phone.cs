namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record Phone
    {
        public string Number { get; }

        public Phone(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("Phone number is required.");

            Number = new string(number.Where(char.IsDigit).ToArray());

            if (Number.Length < 10 || Number.Length > 11)
                throw new ArgumentException("Invalid phone number length.");
        }
    }
}
