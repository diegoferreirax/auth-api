using AuthApi.Application.Infrastructure.UnitOfWork;
using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.DeleteUser.v1;

public sealed class DeleteUserHandler(
    UserRepository userRepository,
    IUnitOfWork unitOfWork)
{
    public readonly UserRepository _userRepository = userRepository;
    public readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Execute(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetBy(command.Id, cancellationToken);
        if (!user.HasValue)
        {
            return Result.Failure(AuthApi_Resource.USER_NOT_EXISTS);
        }

        _userRepository.Delete(user.Value);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
