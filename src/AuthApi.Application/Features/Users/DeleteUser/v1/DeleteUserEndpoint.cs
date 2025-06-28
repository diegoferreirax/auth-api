using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Application.Features.Users.DeleteUser.v1;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class DeleteUserEndpoint : ControllerBase
{
    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(
        Guid id, 
        [FromServices] DeleteUserHandler _handler, 
        CancellationToken cancellationToken)
    {
        var command = DeleteUserCommand.Create(id);
        if (command.IsFailure)
        {
            return BadRequest(command.Error);
        }

        var result = await _handler.Execute(command.Value, cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }
}
