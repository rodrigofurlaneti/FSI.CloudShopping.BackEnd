namespace FSI.CloudShopping.Domain.ValueObjects;

using FSI.CloudShopping.Domain.Core;

/// <summary>
/// Value object representing a Brazilian CNPJ (Cadastro Nacional da Pessoa Jurídica) - 14 digits.
/// </summary>
public class BusinessTaxId : ValueObject
{
    public string Value { get; }

    public BusinessTaxId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("CNPJ cannot be empty.", nameof(value));

        var cleaned = value.Replace(".", "").Replace("-", "").Replace("/", "");

        if (cleaned.Length != 14 || !cleaned.All(char.IsDigit))
            throw new ArgumentException("CNPJ must contain exactly 14 digits.", nameof(value));

        if (!IsValidCnpj(cleaned))
            throw new ArgumentException("Invalid CNPJ.", nameof(value));

        Value = cleaned;
    }

    private static bool IsValidCnpj(string cnpj)
    {
        if (cnpj == new string(cnpj[0], 14))
            return false;

        var length = cnpj.Length - 2;
        var numbers = cnpj.Substring(0, length);
        var digits = cnpj.Substring(length);

        var sum = 0;
        var position = 0;
        for (var i = length - 1; i >= 0; i--)
        {
            sum += int.Parse(numbers[position].ToString()) * (i % 8 + 2);
            position++;
        }

        var result = sum % 11 < 2 ? 0 : 11 - sum % 11;
        if (result != int.Parse(digits[0].ToString()))
            return false;

        sum = 0;
        position = 0;
        for (var i = length + 1 - 1; i >= 0; i--)
        {
            sum += int.Parse(cnpj[position].ToString()) * (i % 8 + 2);
            position++;
        }

        result = sum % 11 < 2 ? 0 : 11 - sum % 11;
        return result == int.Parse(digits[1].ToString());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
