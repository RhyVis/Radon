using Microsoft.Extensions.Configuration;
using NLog;
using Radon.Common.Core.Extension;

namespace Radon.Common.Core.Config;

public static class AppSettings
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private static AppSettingsObject? _appSettings;

    public static void LoadConfiguration(IConfiguration? configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (_appSettings is not null) throw new ArgumentException("AppSettings already loaded.");

        _appSettings = new AppSettingsObject();
        configuration.GetSection("Radon").Bind(_appSettings);

        if (_appSettings.HasNullProperties()) throw new FileLoadException("AppSettings contains null properties.");

        Logger.Debug("AppSettings successfully sealed.");
    }

    public static AppSettingsObject Get()
    {
        return _appSettings
               ?? throw new FileLoadException(
                   "AppSettings not loaded. Call LoadConfiguration first."
               );
    }
}