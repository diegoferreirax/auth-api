using AuthApi.Application.Infrastructure.Data;
using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users;

public sealed class User : BaseEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Hash { get; private set; }
    public IEnumerable<Role> Roles { get; private set; }

    public User()
    { }

    private User(Guid id, string name, string email, IEnumerable<Role> roles)
    {
        Id = id;
        Name = name;
        Email = email;
        Roles = roles;
    }

    public void SetHash(string hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            throw new ArgumentException(AuthApi_Resource.USER_SAVE_ERROR);
        }

        Hash = hash;
    }

    public static Result<User> Create(string name, string email, IEnumerable<Role> roles)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<User>(AuthApi_Resource.NAME_REQUIRED);
        }

        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure<User>(AuthApi_Resource.EMAIL_REQUIRED);
        }

        if (!roles.Any())
        {
            return Result.Failure<User>(AuthApi_Resource.ROLE_REQUIRED);
        }

        return Result.Success(new User(Guid.NewGuid(), name, email, roles));
    }
}