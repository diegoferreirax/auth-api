namespace AuthApi.Infraestructure.Domain;

public sealed class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Hash { get; set; }
    public IEnumerable<Role> Roles { get; set; }
}