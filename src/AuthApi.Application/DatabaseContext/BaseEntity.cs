namespace AuthApi.Application.DatabaseContext;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
}
