namespace AuthApi.Infraestructure.Domain;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
}