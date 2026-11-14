namespace AuthApi.Application.Features.Users.Commands.RegisterUser.v1.Dto;
public record RegisterUserRequestDto(string Name, string Email, string Password, IEnumerable<RegisterRoleRequestDto> Roles);
