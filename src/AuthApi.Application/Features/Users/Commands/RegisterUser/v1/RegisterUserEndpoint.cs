using AuthApi.Application.Features.Users.Commands.RegisterUser.v1.Dto;
using AuthApi.Domain.Enums;
using AuthApi.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Application.Features.Users.Commands.RegisterUser.v1;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class RegisterUserEndpoint : ControllerBase
{
    [Authorize(Roles = RoleCodes.UserManager)]
    [HttpPost]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterUserRequestDto registerUser, 
        [FromServices] RegisterUserHandler _handler, 
        CancellationToken cancellationToken)
    {
        var roles = registerUser.Roles
            .Select(r => RegisterRoleCommand.Create(RoleCode.Create(r.Code)))
            .ToList();

        var command = RegisterUserCommand.Create(
            PersonName.Create(registerUser.Name),
            Email.Create(registerUser.Email),
            Password.Create(registerUser.Password),
            roles);

        var result = await _handler.Execute(command, cancellationToken);
        return Ok(result);
    }
}
