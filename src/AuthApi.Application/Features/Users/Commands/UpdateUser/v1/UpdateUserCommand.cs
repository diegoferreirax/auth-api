using AuthApi.Domain.Exceptions;
using AuthApi.Application.Common.Resource;
using AuthApi.Domain.ValueObjects;

namespace AuthApi.Application.Features.Users.Commands.UpdateUser.v1;

public class UpdateUserCommand
{
    public Guid Id { get; private set; }
    public PersonName Name { get; private set; }
    public Email Email { get; private set; }
    public Password? Password { get; private set; }
    public IEnumerable<UpdateRoleCommand> Roles { get; private set; }

    private UpdateUserCommand(Guid id, PersonName name, Email email, Password? password, IEnumerable<UpdateRoleCommand> roles)
    {
        Id = id;
        Name = name;
        Email = email;
        Password = password;
        Roles = roles;
    }

    public static UpdateUserCommand Create(Guid id, PersonName name, Email email, Password? password, IEnumerable<UpdateRoleCommand> roles)
    {
        if (id == Guid.Empty)
        {
            throw new BusinessException(AuthApi_Resource.USER_NOT_EXISTS);
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
    public RoleCode Code { get; private set; }

    private UpdateRoleCommand(RoleCode code)
    {
        Code = code;
    }

    public static UpdateRoleCommand Create(RoleCode code) => new(code);
}
