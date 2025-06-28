using AuthApi.Application.Infrastructure.Security.Bcrypt;
using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.DeleteUser.v1;

public sealed class DeleteUserHandler
{
    public readonly UserRepository _userRepository;
    public readonly IPasswordHasher _passwordHasher;

    public DeleteUserHandler(
        UserRepository userRepository, 
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result> Execute(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetBy(command.Id);
        if (!user.HasValue)
        {
            return Result.Failure(AuthApi_Resource.USER_NOT_EXISTS);
        }

        await _userRepository.Delete(user.Value);
        return Result.Success();
    }
}
