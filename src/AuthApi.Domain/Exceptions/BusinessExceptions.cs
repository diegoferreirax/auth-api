namespace AuthApi.Application.Exceptions;

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

public class ValidationException : BusinessException
{
    public Dictionary<string, string[]> Errors { get; }
    
    public ValidationException(string message, Dictionary<string, string[]> errors) 
        : base(message, 400)
    {
        Errors = errors;
    }
}

public class NotFoundException : BusinessException
{
    public NotFoundException(string message) : base(message, 404)
    {
    }
}

public class UnauthorizedException : BusinessException
{
    public UnauthorizedException(string message) : base(message, 401)
    {
    }
}

public class ConflictException : BusinessException
{
    public ConflictException(string message) : base(message, 409)
    {
    }
}