using AuthApi.Application.Infrastructure.Data;
using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users;

public sealed class Role : BaseEntity
{
    public string Name { get; private set; } = null!;

    public Role()
    { }

    private Role(string name)
    {
        Name = name;
    }

    public static Result<Role> Create(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<Role>(AuthApi_Resource.ROLE_NAME_REQUIRED);
        }

        return Result.Success(new Role(name));
    }
}