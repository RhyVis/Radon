using System.Reflection;
using System.Text.Json;
using FreeRedis;
using FreeSql;
using FreeSql.Internal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Radon.Common.Core.Config;
using Radon.Common.Core.DI;
using Radon.Core.Model.Base;
using Radon.Data.Entity;
using Radon.Security.Model;
using Scrutor;

namespace Radon.Arc.Setup;

public static class InitService
{
    private const string BaseNamespace = "Radon";
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private static readonly Assembly[] RadonAssemblies =
    [
        // Radon.Common
        typeof(AppSettings).Assembly,
        // Radon.Data
        typeof(BaseEntity).Assembly,
        // Radon.Core
        typeof(BaseApiReq<>).Assembly,
        // Radon.Security
        typeof(Passport).Assembly,
        // Radon.Arc
        typeof(Init).Assembly
    ];

    /// <summary>
    ///     Settings before the application built
    /// </summary>
    public static WebApplicationBuilder SetupService(this WebApplicationBuilder builder)
    {
        builder.Services.SetupSql();
        builder.Services.SetupRedis();
        builder.Services.RegisterService();
        builder
            .Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
        builder.Services.AppendAuthentication();
        builder.Services.AppendMiscService(builder.Environment.IsDevelopment());

        return builder;
    }

    private static void SetupSql(this IServiceCollection services)
    {
        Logger.Debug("Register FreeSql");
        var fsql = new FreeSqlBuilder()
            .UseConnectionString(DataType.PostgreSQL, AppSettings.Get().Data.SQLConnectionString)
            .UseMonitorCommand(cmd =>
                Logger.Info($"Sql：{cmd.CommandText.ReplaceLineEndings(" ")}")
            )
            .UseAutoSyncStructure(true)
            .UseNameConvert(NameConvertType.PascalCaseToUnderscoreWithLower)
            .Build();
        Logger.Debug("Sync Database Structure");
        fsql.CodeFirst.SyncStructure();
        services.AddSingleton(fsql);

        Logger.Debug("Register Repository");
        services.Scan(scan =>
            scan.FromAssemblies(RadonAssemblies)
                .AddClasses(classes =>
                    classes
                        .InNamespaces("Radon")
                        .Where(type =>
                            type.Name.EndsWith("Repository")
                            && !type.Name.StartsWith("Base")
                            && type is { IsClass: true, IsAbstract: false }
                        )
                )
                .AsSelf()
                .WithScopedLifetime()
        );
    }

    private static void SetupRedis(this IServiceCollection services)
    {
        services.AddSingleton<IRedisClient>(_ => new RedisClient(
            AppSettings.Get().Data.RedisConnectionString
        ));
    }

    private static void RegisterService(this IServiceCollection services)
    {
        Logger.Debug("Register Singleton Service");
        services.Scan(scan =>
            scan.FromAssemblies(RadonAssemblies)
                .AddClasses(classes =>
                    classes.InNamespaces(BaseNamespace).WithAttribute<ServiceSingletonAttribute>()
                )
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithSingletonLifetime()
        );
        Logger.Debug("Register Scoped Service");
        services.Scan(scan =>
            scan.FromAssemblies(RadonAssemblies)
                .AddClasses(classes =>
                    classes.InNamespaces(BaseNamespace).WithAttribute<ServiceScopedAttribute>()
                )
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithScopedLifetime()
        );
        Logger.Debug("Register Transient Service");
        services.Scan(scan =>
            scan.FromAssemblies(RadonAssemblies)
                .AddClasses(classes =>
                    classes.InNamespaces(BaseNamespace).WithAttribute<ServiceTransientAttribute>()
                )
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithTransientLifetime()
        );

        Logger.Debug("Register Initializer");
        services.Scan(scan =>
            scan.FromAssemblies(RadonAssemblies)
                .AddClasses(classes =>
                    classes.InNamespaces(BaseNamespace).AssignableTo<IInitializer>()
                )
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelf()
                .WithTransientLifetime()
        );
    }

    private static void AppendAuthentication(this IServiceCollection services)
    {
        Logger.Debug("Register Authentication Service");
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var conf = AppSettings.Get().Security.Jwt;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = conf.Issuer,
                    ValidAudience = conf.Audience,
                    IssuerSigningKey = conf.SigningCredentials.Key
                };
            });
    }

    private static void AppendMiscService(this IServiceCollection services, bool dev)
    {
        Logger.Debug("Register Other Service");
        services.AddHttpClient();

        if (!dev) return;
        services.AddEndpointsApiExplorer();
        services.AddOpenApi();
    }
}