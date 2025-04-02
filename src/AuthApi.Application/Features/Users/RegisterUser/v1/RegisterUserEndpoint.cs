using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Application.Features.Users.RegisterUser.v1;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class RegisterUserEndpoint : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterUserRequest registerUser, 
        [FromServices] RegisterUserHandler _handler, 
        CancellationToken cancellationToken)
    {
        var command = RegisterUserCommand.Create(registerUser.Name, registerUser.Email, registerUser.Password, registerUser.Role);
        if (command.IsFailure)
        {
            return BadRequest(command.Error);
        }

        var result = await _handler.Execute(command.Value, cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}

public record RegisterUserRequest(string Name, string Email, string Password, string Role);
public record RegisterUserResponse(Guid Id);
