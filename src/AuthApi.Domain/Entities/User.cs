using AuthApi.Application.Exceptions;
using AuthApi.Domain.Resource;

namespace AuthApi.Domain.Entities;

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
            throw new BusinessException(AuthApi_Resource.USER_SAVE_ERROR);
        }

        Hash = hash;
    }

    public void UpdateBasicInfo(string name, string email)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new BusinessException(AuthApi_Resource.NAME_REQUIRED);
        }

        if (string.IsNullOrEmpty(email))
        {
            throw new BusinessException(AuthApi_Resource.EMAIL_REQUIRED);
        }

        Name = name;
        Email = email;
    }

    public static User Create(string name, string email)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new BusinessException(AuthApi_Resource.NAME_REQUIRED);
        }

        if (string.IsNullOrEmpty(email))
        {
            throw new BusinessException(AuthApi_Resource.EMAIL_REQUIRED);
        }

        return new User(Guid.NewGuid(), name, email);
    }
}