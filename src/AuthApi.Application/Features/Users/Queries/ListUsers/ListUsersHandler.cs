using AuthApi.Application.Models;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.Queries.ListUsers;

public sealed class ListUsersHandler(UserRepository userRepository)
{
    public readonly UserRepository _userRepository = userRepository;

    public async Task<Result<PaginatedList<ListUsersResponse>>> Execute(PaginationParametersRequest paginationParameters, CancellationToken cancellationToken = default)
    {
        var (users, count) = await _userRepository.GetUsers(paginationParameters.PageNumber, paginationParameters.PageSize, cancellationToken);

        var userResponses = users.Select(user => new ListUsersResponse(
            user.Id,
            user.Name,
            user.Email));

        return Result.Success(new PaginatedList<ListUsersResponse>(userResponses, count, paginationParameters.PageNumber, paginationParameters.PageSize));
    }
}

public record ListUsersResponse(Guid Id, string Name, string Email);