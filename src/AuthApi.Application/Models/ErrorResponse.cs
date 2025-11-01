namespace AuthApi.Application.Models;

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? TraceId { get; set; }
}

public class ValidationErrorResponse : ErrorResponse
{
    public Dictionary<string, string[]> Errors { get; set; } = new();
}
