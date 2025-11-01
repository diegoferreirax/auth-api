using AuthApi.Application.Models;
using AuthApi.Application.Persistence.UnitOfWork;

namespace AuthApi.Application.Features.Users.Queries.ListUsers;

public sealed class ListUsersHandler(IUnitOfWork unitOfWork)
{
    public readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PaginatedList<ListUsersResponse>> Execute(PaginationParametersRequest paginationParameters, CancellationToken cancellationToken = default)
    {
        var (users, count) = await _unitOfWork.Users.GetUsers(paginationParameters.PageNumber, paginationParameters.PageSize, cancellationToken);

        var userResponses = users.Select(user => new ListUsersResponse(
            user.Id,
            user.Name,
            user.Email));

        return new PaginatedList<ListUsersResponse>(userResponses, count, paginationParameters.PageNumber, paginationParameters.PageSize);
    }
}

public record ListUsersResponse(Guid Id, string Name, string Email);