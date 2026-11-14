using AuthApi.Domain.Enums;
using AuthApi.Domain.Exceptions;
using AuthApi.Domain.Resource;

namespace AuthApi.Domain.ValueObjects;

public sealed class RoleCode
{
    public string Value { get; }

    private RoleCode(string value)
    {
        Value = value;
    }

    public static RoleCode Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new BusinessException(AuthApi_Resource.CODE_REQUIRED);
        }

        var trimmed = value.Trim().ToUpperInvariant();

        if (!RoleCodes.IsValid(trimmed))
        {
            throw new BusinessException(AuthApi_Resource.INVALID_ROLE_CODE);
        }

        return new RoleCode(trimmed);
    }

    public override string ToString() => Value;
}
