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

            var cleanedNumber = new string(number.Where(char.IsDigit).ToArray());

            if (cleanedNumber.Length != 14 || cleanedNumber.Distinct().Count() == 1 || !Validate(cleanedNumber))
                throw new DomainException("Número de CNPJ inválido.");

            Number = cleanedNumber;
        }

        private static bool Validate(string cnpj)
        {
            int[] m1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] m2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * m1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * m2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }
    }
}