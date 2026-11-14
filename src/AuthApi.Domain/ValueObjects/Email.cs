using System.Text.RegularExpressions;
using AuthApi.Domain.Exceptions;
using AuthApi.Domain.Resource;

namespace AuthApi.Domain.ValueObjects;

public sealed class Email
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(250));

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new BusinessException(AuthApi_Resource.EMAIL_REQUIRED);
        }

        var trimmed = value.Trim();

        if (trimmed.Length > 150)
        {
            throw new BusinessException(AuthApi_Resource.INVALID_EMAIL_FORMAT);
        }

        if (!EmailRegex.IsMatch(trimmed))
        {
            throw new BusinessException(AuthApi_Resource.INVALID_EMAIL_FORMAT);
        }

        return new Email(trimmed.ToLowerInvariant());
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) => obj is Email other && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode(StringComparison.Ordinal);
}
