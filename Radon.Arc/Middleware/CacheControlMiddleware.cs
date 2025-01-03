﻿using Microsoft.Extensions.Primitives;

namespace Radon.Arc.Middleware;

public class CacheControlMiddleware(RequestDelegate next)
{
    private static readonly StringValues NoCache =
        new(["no-cache", "no-store", "must-revalidate"]);

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/index.html") context.Response.Headers.Append("Cache-Control", NoCache);

        await next(context);
    }
}