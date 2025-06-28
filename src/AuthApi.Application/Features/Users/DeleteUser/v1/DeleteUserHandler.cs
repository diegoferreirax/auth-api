using AuthApi.Application.Infrastructure.UnitOfWork;
using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.DeleteUser.v1;

public sealed class DeleteUserHandler
{
    public readonly UserRepository _userRepository;
    public readonly IUnitOfWork _unitOfWork;

    public DeleteUserHandler(
        UserRepository userRepository, 
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Execute(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetBy(command.Id);
        if (!user.HasValue)
        {
            return Result.Failure(AuthApi_Resource.USER_NOT_EXISTS);
        }

        _userRepository.Delete(user.Value);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
