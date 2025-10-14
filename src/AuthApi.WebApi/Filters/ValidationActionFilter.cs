using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthApi.WebApi.Filters;

public class ValidationActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = new Dictionary<string, string[]>();

            foreach (var modelState in context.ModelState)
            {
                var key = modelState.Key;
                var errorMessages = modelState.Value.Errors
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                if (errorMessages.Length > 0)
                {
                    errors[key] = errorMessages;
                }
            }

            context.HttpContext.Items["ValidationErrors"] = errors;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
