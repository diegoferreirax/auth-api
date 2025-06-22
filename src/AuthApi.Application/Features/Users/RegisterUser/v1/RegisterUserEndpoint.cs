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
        var roles = new List<RegisterRoleCommand>();

        foreach (var item in registerUser.Role)
        {
            roles.Add(new RegisterRoleCommand { Name = item.Name });
        }

        var command = RegisterUserCommand.Create(registerUser.Name, registerUser.Email, registerUser.Password, roles);
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

public record RegisterRoleRequest(string Name);
public record RegisterUserRequest(string Name, string Email, string Password, IEnumerable<RegisterRoleRequest> Role);
public record RegisterUserResponse(Guid Id);
