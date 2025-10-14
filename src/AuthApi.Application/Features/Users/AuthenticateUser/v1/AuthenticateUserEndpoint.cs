using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Application.Features.Users.AuthenticateUser.v1;

[ApiController]
[Route("api/v{version:apiVersion}/users/authenticate")]
[ApiVersion("1.0")]
public class AuthenticateUserEndpoint : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AuthenticateUser(
        [FromBody] AuthenticateUserRequest authenticateUser,
        [FromServices] AuthenticateUserHandler _handler,
        CancellationToken cancellationToken)
    {
        var command = AuthenticateUserCommand.Create(authenticateUser.Email, authenticateUser.Password);
        var result = await _handler.Execute(command, cancellationToken);
        return Ok(result);
    }
}

public record AuthenticateUserRequest(string Email, string Password);
public record AuthenticateUserResponse(string Email, string Token);