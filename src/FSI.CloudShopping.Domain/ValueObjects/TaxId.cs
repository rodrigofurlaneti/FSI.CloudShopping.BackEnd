namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record TaxId
    {
        public string Number { get; }
        public bool IsCompany { get; }

        public TaxId(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("TaxId number cannot be empty.");

            Number = Clean(number);
            IsCompany = Number.Length == 14;

            Validate();
        }

        private string Clean(string input) =>
            new string(input.Where(char.IsDigit).ToArray());

        private void Validate()
        {
            if (Number.Length != 11 && Number.Length != 14)
                throw new ArgumentException("TaxId must be 11 (CPF) or 14 (CNPJ) digits.");
        }
    }
}
