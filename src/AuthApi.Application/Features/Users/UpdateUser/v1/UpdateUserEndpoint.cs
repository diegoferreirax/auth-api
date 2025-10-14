using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Application.Features.Users.UpdateUser.v1;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class UpdateUserEndpoint : ControllerBase
{
    [Authorize(Roles = "A")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(
        Guid id,
        [FromBody] UpdateUserRequest updateUser,
        [FromServices] UpdateUserHandler _handler,
        CancellationToken cancellationToken)
    {
        var roles = new List<UpdateRoleCommand>();
        foreach (var item in updateUser.Roles)
        {
            roles.Add(UpdateRoleCommand.Create(item.Code).Value);
        }

        var command = UpdateUserCommand.Create(id, updateUser.Name, updateUser.Email, updateUser.Password, roles);
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

public record UpdateRoleRequest(string Code);
public record UpdateUserRequest(string Name, string Email, string? Password, IEnumerable<UpdateRoleRequest> Roles);
