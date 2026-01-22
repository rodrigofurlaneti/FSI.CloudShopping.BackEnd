using FSI.CloudShopping.Domain.Core;

namespace FSI.CloudShopping.Domain.ValueObjects;

public record TaxId
{
    public string Number { get; }
    public bool IsCompany { get; }

    public TaxId(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new DomainException("TaxId number cannot be empty.");

        Number = new string(number.Where(char.IsDigit).ToArray());
        IsCompany = Number.Length == 14;

        if (!IsValid())
            throw new DomainException($"Invalid {(IsCompany ? "CNPJ" : "CPF")} number.");
    }

    private bool IsValid()
    {
        if (Number.Length != 11 && Number.Length != 14) return false;
        if (Number.Distinct().Count() == 1) return false;

        return IsCompany ? ValidateCnpj(Number) : ValidateCpf(Number);
    }

    private static bool ValidateCpf(string cpf)
    {
        int[] m1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] m2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        return ValidateDocument(cpf, 9, m1, m2);
    }

    private static bool ValidateCnpj(string cnpj)
    {
        int[] m1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] m2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        return ValidateDocument(cnpj, 12, m1, m2);
    }

    private static bool ValidateDocument(string doc, int baseLength, int[] m1, int[] m2)
    {
        string seed = doc.Substring(0, baseLength);
        string d1 = CalculateDigit(seed, m1);
        string d2 = CalculateDigit(seed + d1, m2);
        return doc.EndsWith(d1 + d2);
    }

    private static string CalculateDigit(string baseNum, int[] weights)
    {
        int sum = 0;
        for (int i = 0; i < weights.Length; i++)
            sum += (baseNum[i] - '0') * weights[i];

        int remainder = sum % 11;
        int digit = remainder < 2 ? 0 : 11 - remainder;
        return digit.ToString();
    }

    public override string ToString() => Number;
}