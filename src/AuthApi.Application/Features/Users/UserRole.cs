using AuthApi.Application.Exceptions;
using AuthApi.Application.Resource;

namespace AuthApi.Application.Features.Users;

public sealed class UserRole
{
    public Guid Id { get; private set; }
    public Guid IdUser { get; private set; }
    public Guid IdRole { get; private set; }

    public UserRole()
    { }

    private UserRole(Guid idUser, Guid idRole)
    {
        Id = Guid.NewGuid();
        IdUser = idUser;
        IdRole = idRole;
    }

    public static UserRole Create(Guid idUser, Guid idRole)
    {
        if (idUser == Guid.Empty)
        {
            throw new BusinessException(AuthApi_Resource.USER_ID_CANNOT_BE_EMPTY_GUID);
        }

        if (idRole == Guid.Empty)
        {
            throw new BusinessException(AuthApi_Resource.ROLE_ID_CANNOT_BE_EMPTY_GUID);
        }

        return new UserRole(idUser, idRole);
    }
}