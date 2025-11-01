namespace AuthApi.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
}
