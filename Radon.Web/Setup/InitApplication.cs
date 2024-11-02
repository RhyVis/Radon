using NLog;
using Radon.Common.Core.DI;
using Radon.Common.Core.Extension;
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

        app.UseAuthorization();

        app.UseStaticFiles();
        app.UseRouting();
        app.MapControllers();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseOpenApi(options =>
            {
                options.Path = "/api/openapi/radon.json";
                options.DocumentName = "radon";
            });
            app.MapScalarApiReference();
        }

        app.MapFallbackToFile("index.html");

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
                _logger.Debug($"Trying to Initialize {service.GetType().Name}");
                service.InitAsync().GetAwaiter().GetResult();
            }
        }
        catch (Exception e)
        {
            _logger.Error(e, "Initializer Error on Startup");
        }
        finally
        {
            scope.Dispose();
        }
    }
}
