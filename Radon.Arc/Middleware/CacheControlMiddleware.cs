namespace Radon.Arc.Middleware;

public class CacheControlMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/index.html")
        {
            context.Response.Headers.Append("Cache-Control", "no-cache, no-store");
        }

        await next(context);
    }
}
