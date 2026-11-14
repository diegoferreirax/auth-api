using AuthApi.Application.Features.Users.Commands.AuthenticateUser.v1.Dto;
using AuthApi.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Application.Features.Users.Commands.AuthenticateUser.v1;

[ApiController]
[Route("api/v{version:apiVersion}/users/authenticate")]
[ApiVersion("1.0")]
public class AuthenticateUserEndpoint : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AuthenticateUser(
        [FromBody] AuthenticateUserRequestDto authenticateUser,
        [FromServices] AuthenticateUserHandler _handler,
        CancellationToken cancellationToken)
    {
        var command = AuthenticateUserCommand.Create(
            Email.Create(authenticateUser.Email),
            Password.Create(authenticateUser.Password));
        var result = await _handler.Execute(command, cancellationToken);
        return Ok(result);
    }
}