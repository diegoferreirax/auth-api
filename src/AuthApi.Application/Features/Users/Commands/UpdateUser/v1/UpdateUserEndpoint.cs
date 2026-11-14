using AuthApi.Application.Features.Users.Commands.UpdateUser.v1.Dto;
using AuthApi.Domain.Enums;
using AuthApi.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Application.Features.Users.Commands.UpdateUser.v1;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class UpdateUserEndpoint : ControllerBase
{
    [Authorize(Roles = RoleCodes.UserManager)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(
        Guid id,
        [FromBody] UpdateUserRequestDto updateUser,
        [FromServices] UpdateUserHandler _handler,
        CancellationToken cancellationToken)
    {
        var roles = updateUser.Roles
            .Select(r => UpdateRoleCommand.Create(RoleCode.Create(r.Code)))
            .ToList();

        var password = string.IsNullOrEmpty(updateUser.Password) ? null : Password.Create(updateUser.Password);
        var command = UpdateUserCommand.Create(
            id,
            PersonName.Create(updateUser.Name),
            Email.Create(updateUser.Email),
            password,
            roles);
        var result = await _handler.Execute(command, cancellationToken);
        return Ok(result);
    }
}
