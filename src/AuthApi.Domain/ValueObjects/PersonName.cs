using AuthApi.Domain.Exceptions;
using AuthApi.Domain.Resource;

namespace AuthApi.Domain.ValueObjects;

public sealed class PersonName
{
    private const int MaxLength = 150;

    public string Value { get; }

    private PersonName(string value)
    {
        Value = value;
    }

    public static PersonName Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new BusinessException(AuthApi_Resource.NAME_REQUIRED);
        }

        var trimmed = value.Trim();

        if (trimmed.Length > MaxLength)
        {
            throw new BusinessException(AuthApi_Resource.NAME_EXCEEDS_MAX_LENGTH);
        }

        return new PersonName(trimmed);
    }

    public override string ToString() => Value;
}
