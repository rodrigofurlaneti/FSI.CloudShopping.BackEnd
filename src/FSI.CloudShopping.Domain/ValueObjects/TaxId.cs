namespace FSI.CloudShopping.Domain.ValueObjects;

using FSI.CloudShopping.Domain.Core;

/// <summary>
/// Value object representing a Brazilian CPF (Cadastro de Pessoas Físicas) - 11 digits.
/// </summary>
public class TaxId : ValueObject
{
    public string Value { get; }

    public TaxId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("CPF cannot be empty.", nameof(value));

        var cleaned = value.Replace(".", "").Replace("-", "");

        if (cleaned.Length != 11 || !cleaned.All(char.IsDigit))
            throw new ArgumentException("CPF must contain exactly 11 digits.", nameof(value));

        if (!IsValidCpf(cleaned))
            throw new ArgumentException("Invalid CPF.", nameof(value));

        Value = cleaned;
    }

    private static bool IsValidCpf(string cpf)
    {
        if (cpf == new string(cpf[0], 11))
            return false;

        var sum = 0;
        for (var i = 0; i < 9; i++)
        {
            sum += int.Parse(cpf[i].ToString()) * (10 - i);
        }

        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;

        sum = 0;
        for (var i = 0; i < 10; i++)
        {
            sum += int.Parse(cpf[i].ToString()) * (11 - i);
        }

        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;

        return cpf[9] == (char)('0' + digit1) && cpf[10] == (char)('0' + digit2);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
