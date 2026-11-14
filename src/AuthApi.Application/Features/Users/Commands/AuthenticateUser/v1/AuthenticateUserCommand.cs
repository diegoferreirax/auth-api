using AuthApi.Domain.ValueObjects;

namespace AuthApi.Application.Features.Users.Commands.AuthenticateUser.v1;

public class AuthenticateUserCommand
{
    public Email Email { get; private set; }
    public Password Password { get; private set; }

    private AuthenticateUserCommand(Email email, Password password)
    {
        Email = email;
        Password = password;
    }

    public static AuthenticateUserCommand Create(Email email, Password password) =>
        new(email, password);
}
