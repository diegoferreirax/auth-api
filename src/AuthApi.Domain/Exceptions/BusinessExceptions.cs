namespace AuthApi.Domain.Exceptions;

public class BusinessException : Exception
{
    public int StatusCode { get; }
    
    public BusinessException(string message, int statusCode = 400) : base(message)
    {
        StatusCode = statusCode;
    }
    
    public BusinessException(string message, Exception innerException, int statusCode = 400) 
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}

public class ValidationException(string message, Dictionary<string, string[]> errors) : BusinessException(message, 400)
{
    public Dictionary<string, string[]> Errors { get; } = errors;
}

public class NotFoundException(string message) : BusinessException(message, 404)
{
}

public class UnauthorizedException(string message) : BusinessException(message, 401)
{
}

public class ConflictException(string message) : BusinessException(message, 409)
{
}