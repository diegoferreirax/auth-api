using AuthApi.Application.Common.Resource;
using AuthApi.Domain.Exceptions;
using AuthApi.Domain.ValueObjects;

namespace AuthApi.Application.Features.Users.Commands.RegisterUser.v1;

public class RegisterUserCommand
{
    public PersonName Name { get; private set; }
    public Email Email { get; private set; }
    public Password Password { get; private set; }
    public IEnumerable<RegisterRoleCommand> Roles { get; private set; }

    private RegisterUserCommand(PersonName name, Email email, Password password, IEnumerable<RegisterRoleCommand> roles)
    {
        Name = name;
        Email = email;
        Password = password;
        Roles = roles;
    }

    public static RegisterUserCommand Create(PersonName name, Email email, Password password, IEnumerable<RegisterRoleCommand> roles)
    {
        if (roles == null || !roles.Any())
        {
            throw new BusinessException(AuthApi_Resource.ROLE_REQUIRED);
        }

        return new RegisterUserCommand(name, email, password, roles);
    }
}

public class RegisterRoleCommand
{
    public RoleCode Code { get; private set; }

    private RegisterRoleCommand(RoleCode code)
    {
        Code = code;
    }

    public static RegisterRoleCommand Create(RoleCode code) => new(code);
}
