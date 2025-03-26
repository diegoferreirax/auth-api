using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Application.Features.Users.RegisterUser.v1;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class RegisterUserEndpoint : ControllerBase
{
    private readonly RegisterUserHandler _handler;

    public RegisterUserEndpoint(RegisterUserHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest registerUser, CancellationToken cancellationToken)
    {
        return Ok(Users.User.Create(Guid.NewGuid(), registerUser.name, registerUser.email, registerUser.password, "employee").Value);

        //var command = CriarPropostaExemploCommand.Criar(
        //    request.NomeCliente,
        //    request.Valor,
        //    request.Cliente.Nome, request.Cliente.CPF, request.Cliente.Rua, request.Cliente.Cidade, request.Cliente.Estado, request.Cliente.CEP,
        //    request.CreditoConsignado.Banco, request.CreditoConsignado.NumeroParcelas, request.CreditoConsignado.TaxaJuros);

        //if (command.IsFailure)
        //{
        //    return BadRequest(command.Error);
        //}

        //var result = await _handler.Executar(command.Value, cancellationToken);
        //if (result.IsSuccess)
        //{
        //    return Ok(result.Value);
        //}
    }
}

public record RegisterUserRequest(string name, string email, string password);