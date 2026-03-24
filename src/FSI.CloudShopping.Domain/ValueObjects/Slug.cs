namespace FSI.CloudShopping.Domain.ValueObjects;

using System.Text;
using System.Text.RegularExpressions;
using FSI.CloudShopping.Domain.Core;

/// <summary>
/// Value object representing a URL-friendly slug for products and categories.
/// </summary>
public class Slug : ValueObject
{
    public string Value { get; }

    public Slug(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Slug cannot be empty.", nameof(value));

        var slug = NormalizeSlug(value);

        if (slug.Length < 3 || slug.Length > 200)
            throw new ArgumentException("Slug must be between 3 and 200 characters.", nameof(value));

        Value = slug;
    }

    private static string NormalizeSlug(string text)
    {
        var normalized = text
            .Trim()
            .ToLowerInvariant()
            .Replace(" ", "-");

        var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(normalized);
        normalized = Encoding.ASCII.GetString(bytes);

        normalized = Regex.Replace(normalized, @"[^a-z0-9\s\-_]", "");
        normalized = Regex.Replace(normalized, @"[\s_]+", "-");
        normalized = Regex.Replace(normalized, @"-+", "-");
        normalized = normalized.Trim('-');

        return normalized;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
