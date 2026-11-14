using AuthApi.Application.Common.Requests;
using AuthApi.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Application.Features.Users.Queries.ListUsers;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class ListUsersEndpoint : ControllerBase
{
    [Authorize(Roles = RoleCodes.UserManager)]
    [HttpGet]
    public async Task<IActionResult> ListUsers(
        [FromQuery] PaginationParametersRequest paginationParameters, 
        [FromServices] ListUsersHandler _handler, 
        CancellationToken cancellationToken)
    {
        var result = await _handler.Execute(paginationParameters, cancellationToken);
        return Ok(result);
    }
}
