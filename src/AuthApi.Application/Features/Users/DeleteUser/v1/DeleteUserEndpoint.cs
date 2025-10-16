using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Application.Features.Users.DeleteUser.v1;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class DeleteUserEndpoint : ControllerBase
{
    [Authorize(Roles = "MASTER")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(
        Guid id, 
        [FromServices] DeleteUserHandler _handler, 
        CancellationToken cancellationToken)
    {
        var command = DeleteUserCommand.Create(id);
        await _handler.Execute(command, cancellationToken);
        return Ok();
    }
}
