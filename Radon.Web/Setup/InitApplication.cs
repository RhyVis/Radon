using NLog;
using Radon.Common.Core.DI;
using Radon.Common.Core.Extension;
using Radon.Web.Middleware;
using Scalar.AspNetCore;

namespace Radon.Web.Setup;

public static class InitApplication
{
    private static Logger _logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Settings after the application is built
    /// </summary>
    public static WebApplication SetupApplication(this WebApplication app)
    {
        app.SetupInitializer();

        app.UseMiddleware<ExceptionFilterMiddleware>();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapControllers();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseOpenApi(opt =>
        {
            opt.Path = "/api/openapi/{documentName}.json";
        });
        app.MapScalarApiReference(opt =>
        {
            opt.Title = "Radon API";
            opt.EndpointPathPrefix = "/api/scalar/{documentName}";
            opt.OpenApiRoutePattern = "/api/openapi/{documentName}.json";
        });

        app.MapFallbackToFile("/index.html");

        return app;
    }

    private static void SetupInitializer(this IApplicationBuilder app)
    {
        _logger.Info("Starting Initializer");
        var scope = app.ApplicationServices.CreateScope();
        try
        {
            var services = scope.ServiceProvider.GetServicesByInterface<IInitializer>();
            foreach (var service in services)
            {
                try
                {
                    _logger.Debug($"Trying to Initialize {service.GetType().Name}");
                    service.InitAsync().GetAwaiter().GetResult();
                }
                catch (Exception e)
                {
                    _logger.Warn($"Initializer {service.GetType().Name} failed with {e.Message}");
                    _logger.Info($"Retrying to Initialize {service.GetType().Name}");
                    service.InitAsync().GetAwaiter().GetResult();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Initializer Error on Startup");
        }
        finally
        {
            scope.Dispose();
        }
    }
}
