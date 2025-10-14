namespace AuthApi.Infraestructure.Domain;

public sealed class User : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Hash { get; set; } = null!;
    public ICollection<UserRole> UserRoles { get; set; } = null!;
}