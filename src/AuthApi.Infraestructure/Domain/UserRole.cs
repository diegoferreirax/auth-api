namespace AuthApi.Infraestructure.Domain;

public sealed class UserRole
{
    public Guid Id { get; set; }
    public Guid IdUser { get; set; }
    public Guid IdRole { get; set; }
}