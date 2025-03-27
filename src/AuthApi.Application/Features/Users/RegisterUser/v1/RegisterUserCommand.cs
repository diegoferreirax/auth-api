using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.RegisterUser.v1;

public class RegisterUserCommand
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string Role { get; private set; }

    private RegisterUserCommand(string name, string email, string password, string role)
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }

    public static Result<RegisterUserCommand> Create(string name, string email, string password, string role)
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

        if (string.IsNullOrEmpty(role))
        {
            return Result.Failure<RegisterUserCommand>(AuthApi_Resource.ROLE_REQUIRED);
        }

        return Result.Success(new RegisterUserCommand(name, email, password, role));
    }
}
