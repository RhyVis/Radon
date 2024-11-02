using System.Reflection;
using System.Text.Json;
using FreeSql;
using FreeSql.Internal;
using NLog;
using Radon.Common.Core.Config;
using Radon.Common.Core.DI;
using Radon.Core.Model.Base;
using Radon.Data.Entity.Base;
using Scrutor;

namespace Radon.Web.Setup;

public static class InitService
{
    private static Logger _logger = LogManager.GetCurrentClassLogger();
    private const string BASE_NAMESPACE = "Radon";

    private static Assembly[] RADON_ASSEMBLIES = new[]
    {
        // Radon.Common
        typeof(AppSettings).Assembly,
        // Radon.Data
        typeof(BaseEntity).Assembly,
        // Radon.Core
        typeof(BaseApiReq<>).Assembly,
        // Radon.Web
        typeof(Init).Assembly,
    };

    /// <summary>
    /// Settings before the application built
    /// </summary>
    public static WebApplicationBuilder SetupService(this WebApplicationBuilder builder)
    {
        builder.Services.SetupSql();
        builder.Services.RegisterService();
        builder
            .Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
        builder.Services.AddEndpointsApiExplorer();
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddOpenApiDocument(configure =>
            {
                configure.DocumentName = "radon";
                configure.Title = "Radon API";
                configure.Version = "1.0";
            });
        }
        builder.Services.AppendMiscService();

        return builder;
    }

    private static void SetupSql(this IServiceCollection services)
    {
        _logger.Debug("Register FreeSql");
        var fsql = new FreeSqlBuilder()
            .UseConnectionString(DataType.PostgreSQL, AppSettings.Get().SQL.ConnectionString)
            .UseMonitorCommand(cmd =>
                _logger.Info($"Sql：{cmd.CommandText.ReplaceLineEndings(" ")}")
            )
            .UseAutoSyncStructure(true)
            .UseNameConvert(NameConvertType.PascalCaseToUnderscoreWithLower)
            .Build();
        services.AddSingleton(fsql);

        _logger.Debug("Register Repository");
        services.Scan(scan =>
            scan.FromAssemblies(RADON_ASSEMBLIES)
                .AddClasses(classes =>
                    classes
                        .InNamespaces("Radon.Data.Repository")
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

    private static void RegisterService(this IServiceCollection services)
    {
        _logger.Debug("Register Singleton Service");
        services.Scan(scan =>
            scan.FromAssemblies(RADON_ASSEMBLIES)
                .AddClasses(classes =>
                    classes.InNamespaces(BASE_NAMESPACE).WithAttribute<ServiceSingletonAttribute>()
                )
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithSingletonLifetime()
        );
        _logger.Debug("Register Scoped Service");
        services.Scan(scan =>
            scan.FromAssemblies(RADON_ASSEMBLIES)
                .AddClasses(classes =>
                    classes.InNamespaces(BASE_NAMESPACE).WithAttribute<ServiceScopedAttribute>()
                )
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithScopedLifetime()
        );
        _logger.Debug("Register Transient Service");
        services.Scan(scan =>
            scan.FromAssemblies(RADON_ASSEMBLIES)
                .AddClasses(classes =>
                    classes.InNamespaces(BASE_NAMESPACE).WithAttribute<ServiceTransientAttribute>()
                )
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithTransientLifetime()
        );

        _logger.Debug("Register Initializer");
        services.Scan(scan =>
            scan.FromAssemblies(RADON_ASSEMBLIES)
                .AddClasses(classes =>
                    classes.InNamespaces(BASE_NAMESPACE).AssignableTo<IInitializer>()
                )
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelf()
                .WithTransientLifetime()
        );
    }

    private static void AppendMiscService(this IServiceCollection services)
    {
        _logger.Debug("Register Other Service");
        services.AddHttpClient();
    }
}
