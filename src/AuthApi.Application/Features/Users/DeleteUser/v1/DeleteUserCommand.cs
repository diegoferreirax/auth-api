using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.DeleteUser.v1;

public class DeleteUserCommand
{
    public Guid Id { get; private set; }

    private DeleteUserCommand(Guid id)
    {
        Id = id;
    }

    public static Result<DeleteUserCommand> Create(Guid id)
    {
        return Result.Success(new DeleteUserCommand(id));
    }
}
