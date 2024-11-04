using System.Text;
using Masuit.Tools.Security;
using Microsoft.IdentityModel.Tokens;

namespace Radon.Common.Core.Config;

public class AppSettingsObject
{
    private const string DEFAULT_NAME = "Radon";

    public string Name { get; init; } = DEFAULT_NAME;
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

        public JwtSettings Jwt { get; init; } = null!;

        public class JwtSettings
        {
            public string SigningKey { get; init; } = null!;
            public string Issuer { get; init; } = DEFAULT_NAME;
            public string Audience { get; init; } = DEFAULT_NAME;
            public int ExpireMinutes { get; init; }

            public SigningCredentials SigningCredentials =>
                new(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)),
                    SecurityAlgorithms.HmacSha256
                );
        }
    }
}
