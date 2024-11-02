using Microsoft.Extensions.Configuration;
using NLog;
using Radon.Common.Core.Extension;

namespace Radon.Common.Core.Config;

public static class AppSettings
{
    private static Logger _logger = LogManager.GetCurrentClassLogger();
    private static AppSettingsObject? _appSettings;

    public static void LoadConfiguration(IConfiguration? configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (_appSettings != null)
        {
            throw new ArgumentException("AppSettings already loaded.");
        }

        _appSettings = new AppSettingsObject();
        configuration.GetSection("Radon").Bind(_appSettings);

        if (_appSettings.HasNullProperties())
        {
            throw new NullReferenceException("AppSettings contains null properties.");
        }

        _logger.Debug("AppSettings successfully sealed.");
    }

    public static AppSettingsObject Get() =>
        _appSettings
        ?? throw new NullReferenceException(
            "AppSettings not loaded. Call LoadConfiguration first."
        );
}
