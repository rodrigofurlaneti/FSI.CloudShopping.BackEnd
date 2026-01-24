using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValidationAssistant;

namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record TaxId // Focado apenas em CPF
    {
        public string Number { get; }

        public TaxId(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new DomainException("O CPF não pode ser vazio.");

            Number = new string(number.Where(char.IsDigit).ToArray());

            if (Number.Length != 11 || Number.Distinct().Count() == 1 || !Validate())
                throw new DomainException("Número de CPF inválido.");
        }

        private bool Validate()
        {
            int[] m1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] m2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            return DocumentValidator.Validate(Number, 9, m1, m2);
        }

        public override string ToString() => Number;
    }
}