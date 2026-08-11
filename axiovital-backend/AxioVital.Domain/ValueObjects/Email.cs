using System.Text.RegularExpressions;

namespace AxioVital.Domain.ValueObjects;

/// <summary>
/// Value object representing a validated email address.
/// </summary>
public readonly partial record struct Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.", nameof(value));

        if (!EmailRegex().IsMatch(value))
            throw new ArgumentException($"'{value}' is not a valid email address.", nameof(value));

        Value = value.Trim().ToLowerInvariant();
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
    public static explicit operator Email(string value) => new(value);

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
    private static partial Regex EmailRegex();
}
