using AuthApi.Infrastructure.Security.JWT;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Application.Features.Users.AuthenticateUser.v1;

[ApiController]
[Route("api/v{version:apiVersion}/users/authenticate")]
[ApiVersion("1.0")]
public class AuthenticateUserEndpoint : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AuthenticateUser(
        [FromBody] AuthenticateUserRequest authUser,
        [FromServices] UserRepository userRepository,
        [FromServices] TokenService tokenService,
        CancellationToken cancellationToken)
    {
        var user = userRepository.Get(authUser.name, authUser.password);
        if (!user.HasValue)
        {
            return NotFound("Usuário ou senha inválidos.");
        }

        var token = tokenService.GenerateToken(user.Value.Name, user.Value.Role);

        return Ok(new AuthenticateUserResponse(authUser.name, token));
    }

    /*[HttpGet]
    [Route("anonymous")]
    [AllowAnonymous]
    public string Anonymous() => "Anônimo";

    [HttpGet]
    [Route("authenticated")]
    [Authorize]
    public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

    [HttpGet]
    [Route("employee")]
    [Authorize(Roles = "employee,manager")]
    public string Employee() => "Funcionário";

    [HttpGet]
    [Route("manager")]
    [Authorize(Roles = "manager")]
    public string Manager() => "Gerente";*/
}

public record AuthenticateUserRequest(string name, string password);
public record AuthenticateUserResponse(string name, string token);