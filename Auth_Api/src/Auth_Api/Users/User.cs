using Auth_Api.Resource;
using CSharpFunctionalExtensions;

namespace Auth_Api.Users;

public sealed class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string Role { get; private set; }

    private User(Guid id, string name, string email, string password, string role)
    {
        Id = id;
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }

    public static Result<User> Create(Guid id, string name, string email, string password, string role)
    {
        //if (Guid.TryParse(id, out resultId))
        //{
        //    return Result.Failure<User>(Auth_Api_Resource.NAME_REQUIRED);
        //}

        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<User>(Auth_Api_Resource.NAME_REQUIRED);
        }

        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure<User>(Auth_Api_Resource.EMAIL_REQUIRED);
        }

        if (string.IsNullOrEmpty(password))
        {
            return Result.Failure<User>(Auth_Api_Resource.PASSWORD_REQUIRED);
        }

        if (string.IsNullOrEmpty(role))
        {
            return Result.Failure<User>("Role é obrigatória.");
        }

        return Result.Success(new User(id, name, email, password, role));
    }
}
