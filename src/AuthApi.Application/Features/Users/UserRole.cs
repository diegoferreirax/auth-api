using CSharpFunctionalExtensions;

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

    public static Result<UserRole> Create(Guid idUser, Guid idRole)
    {
        if (idUser == Guid.Empty)
        {
            Result.Failure<UserRole>("User ID cannot be an empty GUID.");
        }

        if (idRole == Guid.Empty)
        {
            Result.Failure<UserRole>("Role ID cannot be an empty GUID.");
        }

        return Result.Success(new UserRole(idUser, idRole));
    }
}