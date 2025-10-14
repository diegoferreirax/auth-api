using AuthApi.Application.Resource;
using AuthApi.Application.Exceptions;

namespace AuthApi.Application.Features.Users.RegisterUser.v1;

public class RegisterUserCommand
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public IEnumerable<RegisterRoleCommand> Roles { get; private set; }

    private RegisterUserCommand(string name, string email, string password, IEnumerable<RegisterRoleCommand> roles)
    {
        Name = name;
        Email = email;
        Password = password;
        Roles = roles;
    }

    public static RegisterUserCommand Create(string name, string email, string password, IEnumerable<RegisterRoleCommand> roles)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new BusinessException(AuthApi_Resource.NAME_REQUIRED);
        }

        if (string.IsNullOrEmpty(email))
        {
            throw new BusinessException(AuthApi_Resource.EMAIL_REQUIRED);
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new BusinessException(AuthApi_Resource.PASSWORD_REQUIRED);
        }

        if (!roles.Any())
        {
            throw new BusinessException(AuthApi_Resource.ROLE_REQUIRED);
        }

        return new RegisterUserCommand(name, email, password, roles);
    }
}

public class RegisterRoleCommand
{
    public string Code { get; private set; }

    private RegisterRoleCommand(string code)
    {
        Code = code;
    }

    public static RegisterRoleCommand Create(string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            throw new BusinessException(AuthApi_Resource.ROLE_NAME_REQUIRED);
        }

        return new RegisterRoleCommand(code);
    }
}
