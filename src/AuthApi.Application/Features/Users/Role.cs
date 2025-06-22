using AuthApi.Application.Infrastructure.Data;

namespace AuthApi.Application.Features.Users;

public sealed class Role : BaseEntity
{
    public string Name { get; set; }

    public Role()
    { }
}