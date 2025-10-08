using AuthApi.Application.Persistence.Data;
using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users;

public sealed class User : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Hash { get; private set; } = null!;
    public IEnumerable<UserRole> UserRoles { get; set; } = null!;

    public User()
    { }

    private User(Guid id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public void SetHash(string hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            throw new ArgumentException(AuthApi_Resource.USER_SAVE_ERROR);
        }

        Hash = hash;
    }

    public static Result<User> Create(string name, string email)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<User>(AuthApi_Resource.NAME_REQUIRED);
        }

        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure<User>(AuthApi_Resource.EMAIL_REQUIRED);
        }

        return Result.Success(new User(Guid.NewGuid(), name, email));
    }
}