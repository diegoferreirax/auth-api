using AuthApi.Application.Infrastructure.Data;
using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users;

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

    public static Result<Role> Create(string name, string code)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<Role>(AuthApi_Resource.ROLE_NAME_REQUIRED);
        }
        
        if (string.IsNullOrEmpty(code))
        {
            return Result.Failure<Role>("Code is required");
        }

        return Result.Success(new Role(name, code));
    }
}