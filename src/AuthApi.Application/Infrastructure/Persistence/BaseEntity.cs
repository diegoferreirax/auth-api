namespace AuthApi.Application.Infrastructure.Persistence;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
}
