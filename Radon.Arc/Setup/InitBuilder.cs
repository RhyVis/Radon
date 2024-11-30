using NLog.Web;
using Radon.Common.Core.Config;
using Radon.Common.Utils;

namespace Radon.Arc.Setup;

public static class InitBuilder
{
    /// <summary>
    ///     Setup before services configuration
    /// </summary>
    public static WebApplicationBuilder SetupBuilder(this WebApplicationBuilder builder)
    {
        AppSettings.LoadConfiguration(builder.Configuration);
        InitOpenCC.Setup();

        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        return builder;
    }
}