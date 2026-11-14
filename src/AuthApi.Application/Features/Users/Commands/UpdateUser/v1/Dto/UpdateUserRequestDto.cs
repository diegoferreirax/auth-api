namespace AuthApi.Application.Features.Users.Commands.UpdateUser.v1.Dto;
public record UpdateUserRequestDto(string Name, string Email, string? Password, IEnumerable<UpdateRoleRequestDto> Roles);
