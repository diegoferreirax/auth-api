using AuthApi.Application.Exceptions;
using AuthApi.Domain.Resource;

namespace AuthApi.Domain.Entities;

public sealed class Role : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Code { get; private set; } = null!;

    public Role()
    { }

    private Role(string name, string code)
    {
        Name = name;
        Code = code;
    }

    public static Role Create(string name, string code)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new BusinessException(AuthApi_Resource.ROLE_NAME_REQUIRED);
        }

        if (string.IsNullOrEmpty(code))
        {
            throw new BusinessException(AuthApi_Resource.CODE_REQUIRED);
        }

        return new Role(name, code);
    }
}