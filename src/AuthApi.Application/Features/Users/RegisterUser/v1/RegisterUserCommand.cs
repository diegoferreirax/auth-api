using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

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

    public static Result<RegisterUserCommand> Create(string name, string email, string password, IEnumerable<RegisterRoleCommand> roles)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<RegisterUserCommand>(AuthApi_Resource.NAME_REQUIRED);
        }

        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure<RegisterUserCommand>(AuthApi_Resource.EMAIL_REQUIRED);
        }

        if (string.IsNullOrEmpty(password))
        {
            return Result.Failure<RegisterUserCommand>(AuthApi_Resource.PASSWORD_REQUIRED);
        }

        if (!roles.Any())
        {
            return Result.Failure<RegisterUserCommand>(AuthApi_Resource.ROLE_REQUIRED);
        }

        return Result.Success(new RegisterUserCommand(name, email, password, roles));
    }
}

public class RegisterRoleCommand
{
    public string Name { get; private set; }

    private RegisterRoleCommand(string name)
    {
        Name = name;
    }

    public static Result<RegisterRoleCommand> Create(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<RegisterRoleCommand>(AuthApi_Resource.ROLE_NAME_REQUIRED);
        }

        return Result.Success(new RegisterRoleCommand(name));
    }
}
