namespace AuthApi.Application.Features.Users.DeleteUser.v1;

public class DeleteUserCommand
{
    public Guid Id { get; private set; }

    private DeleteUserCommand(Guid id)
    {
        Id = id;
    }

    public static DeleteUserCommand Create(Guid id)
    {
        return new DeleteUserCommand(id);
    }
}
