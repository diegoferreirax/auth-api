using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Application.Features.Users.UpdateUser.v1;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class UpdateUserEndpoint : ControllerBase
{
    [Authorize(Roles = "UM")]
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
            roles.Add(UpdateRoleCommand.Create(item.Code));
        }

        var command = UpdateUserCommand.Create(id, updateUser.Name, updateUser.Email, updateUser.Password, roles);
        var result = await _handler.Execute(command, cancellationToken);
        return Ok(result);
    }
}

public record UpdateRoleRequest(string Code);
public record UpdateUserRequest(string Name, string Email, string? Password, IEnumerable<UpdateRoleRequest> Roles);
