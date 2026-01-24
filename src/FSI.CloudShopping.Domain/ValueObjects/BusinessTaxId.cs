using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValidationAssistant;
namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record BusinessTaxId 
    {
        public string Number { get; }

        public BusinessTaxId(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new DomainException("O CNPJ não pode ser vazio.");

            Number = new string(number.Where(char.IsDigit).ToArray());

            if (Number.Length != 14 || Number.Distinct().Count() == 1 || !Validate())
                throw new DomainException("Número de CNPJ inválido.");
        }

        private bool Validate()
        {
            int[] m1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] m2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            return DocumentValidator.Validate(Number, 12, m1, m2);
        }

        public override string ToString() => Number;
    }
}