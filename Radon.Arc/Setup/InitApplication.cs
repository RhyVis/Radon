using Microsoft.Extensions.FileProviders;
using NLog;
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
        var scope = app.ApplicationServices.CreateScope();
        try
        {
            var services = scope.ServiceProvider.GetServicesByInterface<IInitializer>();
            foreach (var service in services)
                try
                {
                    Logger.Debug($"Trying to Initialize {service.GetType().Name}");
                    service.InitAsync().GetAwaiter().GetResult();
                }
                catch (Exception e)
                {
                    Logger.Warn($"Initializer {service.GetType().Name} failed with {e.Message}");
                    Logger.Info($"Retrying to Initialize {service.GetType().Name}");
                    service.InitAsync().GetAwaiter().GetResult();
                }
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Initializer Error on Startup");
        }
        finally
        {
            scope.Dispose();
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
