namespace Radon.Common.Core.Config;

public class AppSettingsObject
{
    public string Name { get; init; } = null!;
    public string Version { get; init; } = null!;
    public string ResourceEndpoint { get; init; } = null!;
    public DataSettings Data { get; init; } = null!;

    public SecuritySettings Security { get; init; } = null!;

    public class DataSettings
    {
        public string SQLConnectionString { get; init; } = null!;
        public string RedisConnectionString { get; init; } = null!;
    }

    public class SecuritySettings
    {
        public string AuthToken { get; init; } = null!;
    }
}
