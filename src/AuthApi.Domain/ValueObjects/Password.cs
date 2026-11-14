using AuthApi.Domain.Exceptions;
using AuthApi.Domain.Resource;

namespace AuthApi.Domain.ValueObjects;

public sealed class Password
{
    private const int MinLength = 6;

    public string Value { get; }

    private Password(string value)
    {
        Value = value;
    }

    public static Password Create(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new BusinessException(AuthApi_Resource.PASSWORD_REQUIRED);
        }

        if (value.Length < MinLength)
        {
            throw new BusinessException(AuthApi_Resource.INVALID_PASSWORD_LENGTH);
        }

        return new Password(value);
    }

    public override string ToString() => "********";
}
