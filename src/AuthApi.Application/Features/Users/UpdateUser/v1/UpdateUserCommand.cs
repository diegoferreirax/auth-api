using AuthApi.Application.Resource;
using AuthApi.Application.Exceptions;

namespace AuthApi.Application.Features.Users.UpdateUser.v1;

public class UpdateUserCommand
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string? Password { get; private set; }
    public IEnumerable<UpdateRoleCommand> Roles { get; private set; }

    private UpdateUserCommand(Guid id, string name, string email, string? password, IEnumerable<UpdateRoleCommand> roles)
    {
        Id = id;
        Name = name;
        Email = email;
        Password = password;
        Roles = roles;
    }

    public static UpdateUserCommand Create(Guid id, string name, string email, string? password, IEnumerable<UpdateRoleCommand> roles)
    {
        if (id == Guid.Empty)
        {
            throw new BusinessException(AuthApi_Resource.USER_NOT_EXISTS);
        }

        if (string.IsNullOrEmpty(name))
        {
            throw new BusinessException(AuthApi_Resource.NAME_REQUIRED);
        }

        if (string.IsNullOrEmpty(email))
        {
            throw new BusinessException(AuthApi_Resource.EMAIL_REQUIRED);
        }

        if (roles == null || !roles.Any())
        {
            throw new BusinessException(AuthApi_Resource.ROLE_REQUIRED);
        }

        return new UpdateUserCommand(id, name, email, password, roles);
    }
}

public class UpdateRoleCommand
{
    public string Code { get; private set; }

    private UpdateRoleCommand(string code)
    {
        Code = code;
    }

    public static UpdateRoleCommand Create(string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            throw new BusinessException(AuthApi_Resource.ROLE_NAME_REQUIRED);
        }

        return new UpdateRoleCommand(code);
    }
}
