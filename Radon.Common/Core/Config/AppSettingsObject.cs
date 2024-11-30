using System.Text;
using JetBrains.Annotations;
using Microsoft.IdentityModel.Tokens;

namespace Radon.Common.Core.Config;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class AppSettingsObject
{
    private const string DefaultName = "Radon";

    public string Name { get; init; } = DefaultName;
    public string Version { get; init; } = null!;
    public string ResourceEndpoint { get; init; } = null!;
    public DataSettings Data { get; init; } = null!;
    public SecuritySettings Security { get; init; } = null!;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class DataSettings
    {
        public string SQLConnectionString { get; init; } = null!;
        public string RedisConnectionString { get; init; } = null!;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class SecuritySettings
    {
        public JwtSettings Jwt { get; init; } = null!;

        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        public class JwtSettings
        {
            public string SigningKey { get; init; } = null!;
            public string Issuer { get; init; } = DefaultName;
            public string Audience { get; init; } = DefaultName;
            public int ExpireMinutes { get; init; }

            public SigningCredentials SigningCredentials =>
                new(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)),
                    SecurityAlgorithms.HmacSha256
                );
        }
    }
}