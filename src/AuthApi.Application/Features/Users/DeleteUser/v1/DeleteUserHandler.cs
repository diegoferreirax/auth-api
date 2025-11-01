using AuthApi.Application.Persistence.UnitOfWork;
using AuthApi.Application.Resource;
using AuthApi.Application.Exceptions;

namespace AuthApi.Application.Features.Users.DeleteUser.v1;

public sealed class DeleteUserHandler(IUnitOfWork unitOfWork)
{
    public readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetBy(command.Id, cancellationToken);
        if (!user.HasValue)
        {
            throw new NotFoundException(AuthApi_Resource.USER_NOT_EXISTS);
        }

        _unitOfWork.Users.Delete(user.Value);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
