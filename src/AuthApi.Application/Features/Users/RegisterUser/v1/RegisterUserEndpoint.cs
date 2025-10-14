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
        foreach (var item in registerUser.Roles)
        {
            roles.Add(RegisterRoleCommand.Create(item.Code));
        }

        var command = RegisterUserCommand.Create(registerUser.Name, registerUser.Email, registerUser.Password, roles);
        var result = await _handler.Execute(command, cancellationToken);
        return Ok(result);
    }
}

public record RegisterRoleRequest(string Code);
public record RegisterUserRequest(string Name, string Email, string Password, IEnumerable<RegisterRoleRequest> Roles);
public record RegisterUserResponse(Guid Id);
