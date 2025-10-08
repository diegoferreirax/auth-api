using AuthApi.Application.Persistence.UnitOfWork;
using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.DeleteUser.v1;

public sealed class DeleteUserHandler(IUnitOfWork unitOfWork)
{
    public readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Execute(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetBy(command.Id, cancellationToken);
        if (!user.HasValue)
        {
            return Result.Failure(AuthApi_Resource.USER_NOT_EXISTS);
        }

        _unitOfWork.Users.Delete(user.Value);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
