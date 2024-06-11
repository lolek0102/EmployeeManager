using System.Text.Json;

namespace EmployeeManager.Application.Middleware;
public class CustomErrorHandlingMiddleware(RequestDelegate _next, ILogger<CustomErrorHandlingMiddleware> _logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            switch (context.Response.StatusCode)
            {
                case StatusCodes.Status400BadRequest:
                    await HandleBadRequestResponse(context);
                    break;
                case StatusCodes.Status401Unauthorized:
                    await HandleUnauthorizedResponse(context);
                    break;
                case StatusCodes.Status403Forbidden:
                    await HandleForbiddenResponse(context);
                    break;
                case StatusCodes.Status500InternalServerError:
                    await HandleServerErrorResponse(context);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in request pipeline.");
            await HandleServerErrorResponse(context);
        }
    }


    private static Task HandleBadRequestResponse(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            Message = "Request payload was invalid."
        };
        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }

    private static Task HandleUnauthorizedResponse(HttpContext context)
    {
        var response = new
        {
            Message = "You need to be logged in to access this resource."
        };
        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }


    private static Task HandleForbiddenResponse(HttpContext context)
    {
        var response = new
        {
            Message = "You don't have permission to access this resource."
        };
        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }

    private static Task HandleServerErrorResponse(HttpContext context)
    {
        var response = new
        {
            Message = "There was an unexpected server-side error."
        };
        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}
