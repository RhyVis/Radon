﻿using System.Text.Json;
using System.Text.Json.Serialization;
using NLog;
using Radon.Core.Model.Response;
using Radon.Security.Exceptions;

namespace Radon.Arc.Middleware;

public class ExceptionFilterMiddleware(RequestDelegate next)
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private static readonly JsonSerializerOptions JsonSerializerOptions =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new ExceptionConverter() }
        };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BaseSecurityException ex)
        {
            Logger.Error(ex, "User authentication failed.");
            await HandleAuthenticationAsync(context, ex);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "An error occurred while processing requests.");
            await HandleUnknownAsync(context, ex);
        }
    }

    private static Task HandleAuthenticationAsync(HttpContext context, Exception exception)
    {
        var response = new ExceptionRes
        {
            Code = StatusCodes.Status401Unauthorized,
            Msg = "Mismatched username or password.",
            Data = JsonSerializer.Serialize(exception, JsonSerializerOptions)
        };
        return WriteResponseAsync(context, response, StatusCodes.Status401Unauthorized);
    }

    private static Task HandleUnknownAsync(HttpContext context, Exception exception)
    {
        var response = new ExceptionRes
        {
            Code = StatusCodes.Status500InternalServerError,
            Msg = "An error occurred while processing your request.",
            Data = JsonSerializer.Serialize(exception, JsonSerializerOptions)
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

        var json = JsonSerializer.Serialize(response, JsonSerializerOptions);

        return context.Response.WriteAsync(json);
    }

    private sealed class ExceptionConverter : JsonConverter<Exception>
    {
        public override Exception Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            return new Exception();
        }

        public override void Write(
            Utf8JsonWriter writer,
            Exception value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartObject();
            writer.WriteString("type", value.GetType().ToString());
            writer.WriteString("message", value.Message);
            writer.WriteString("stackTrace", value.StackTrace);
            if (value.InnerException != null)
            {
                writer.WritePropertyName("innerException");
                JsonSerializer.Serialize(writer, value.InnerException, options);
            }

            writer.WriteEndObject();
        }
    }
}
