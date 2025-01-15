using Microsoft.Extensions.FileProviders;
using NLog;
using NLog.Fluent;
using Radon.Arc.Middleware;
using Radon.Common.Core.DI;
using Radon.Common.Core.Extension;
using Scalar.AspNetCore;

namespace Radon.Arc.Setup;

public static class InitApplication
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    ///     Settings after the application is built
    /// </summary>
    public static WebApplication SetupApplication(this WebApplication app)
    {
        app.SetupInitializer();

        app.UseMiddleware<ExceptionFilterMiddleware>();
        app.UseMiddleware<CacheControlMiddleware>();

        app.UseStaticFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider =
                new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "appStatic")),
            RequestPath = new PathString("/static")
        });

        app.UseRouting();
        app.MapControllers();

        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi("/api/openapi/{documentName}.json");
            app.MapScalarApiReference(opt =>
            {
                opt.Title = "Radon API";
                opt.EndpointPathPrefix = "/api/scalar/{documentName}";
                opt.OpenApiRoutePattern = "/api/openapi/{documentName}.json";
            });
        }

        app.MapFallbackToFile("/index.html");

        if (app.Environment.IsDevelopment()) app.DevTest();

        return app;
    }

    private static void SetupInitializer(this IApplicationBuilder app)
    {
        Logger.Info("Starting Initializer");
        using var scope = app.ApplicationServices.CreateScope();
        try
        {
            var services = scope.ServiceProvider.GetServicesByInterface<IInitializer>();
            foreach (var service in services)
            {
                var initialized = false;
                for (var attempt = 1; attempt <= 5; attempt++)
                {
                    try
                    {
                        Logger.Debug($"Trying to Initialize {service.GetType().Name}");
                        service.InitAsync().GetAwaiter().GetResult();
                        initialized = true;
                        break;
                    }
                    catch (Exception e)
                    {
                        Logger.Warn(e, $"Initializer {service.GetType().Name} failed");
                        if (attempt >= 5)
                            Logger.Error(e, $"Initializer {service.GetType().Name} failed after 5 attempts");
                    }
                }

                if (!initialized)
                {
                    throw new Exception($"Failed to Initialize {service.GetType().Name} after 5 attempts");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Initializer Error on Startup");
        }
    }

    private static void DevTest(this IApplicationBuilder app)
    {
        Logger.Info("Starting tester");
        var scope = app.ApplicationServices.CreateScope();
        try
        {
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Test err");
            throw;
        }
        finally
        {
            scope.Dispose();
        }
    }
}
