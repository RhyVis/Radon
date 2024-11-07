using System.Text.Json;
using Radon.Core.Model.Response;
using Radon.Security.Exceptions;

namespace Radon.Web.Middleware;

public class ExceptionFilterMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BaseSecurityException ex)
        {
            await HandleAuthenticationAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleUnknownAsync(context, ex);
        }
    }

    private static Task HandleAuthenticationAsync(HttpContext context, Exception exception)
    {
        var response = new ExceptionRes
        {
            Code = StatusCodes.Status401Unauthorized,
            Msg = "Mismatched username or password.",
            Data = exception.Message,
        };
        return WriteResponseAsync(context, response, StatusCodes.Status401Unauthorized);
    }

    private static Task HandleUnknownAsync(HttpContext context, Exception exception)
    {
        var response = new ExceptionRes
        {
            Code = StatusCodes.Status500InternalServerError,
            Msg = "An error occurred while processing your request.",
            Data = exception.Message,
        };
        return WriteResponseAsync(context, response);
    }

    private static Task WriteResponseAsync(
        HttpContext context,
        ExceptionRes response,
        int code = StatusCodes.Status500InternalServerError
    )
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;

        var json = JsonSerializer.Serialize(response, _jsonSerializerOptions);

        return context.Response.WriteAsync(json);
    }

    private static readonly JsonSerializerOptions _jsonSerializerOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
}
