using AuthApi.Application.Models;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.ListUsers.v1;

public sealed class ListUsersHandler
{
    public readonly UserRepository _userRepository;

    public ListUsersHandler(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<PaginatedList<ListUsersResponse>>> Execute(PaginationParameters paginationParameters, CancellationToken cancellationToken = default)
    {
        var (users, count) = await _userRepository.GetUsers(paginationParameters.PageNumber, paginationParameters.PageSize);

        var userResponses = users.Select(user => new ListUsersResponse(
            user.Id,
            user.Name,
            user.Email));

        var usersPaginated = new PaginatedList<ListUsersResponse>(userResponses, count, paginationParameters.PageNumber, paginationParameters.PageSize);
        return Result.Success(usersPaginated);
    }
}

public record ListUsersResponse(Guid Id, string Name, string Email);