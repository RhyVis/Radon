namespace Radon.Common.Core.Extension;

public static class ServiceProviderExtension
{
    /// <summary>
    /// Get all services that implement the interface
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <typeparam name="T">Interface</typeparam>
    /// <returns></returns>
    public static IEnumerable<T> GetServicesByInterface<T>(this IServiceProvider serviceProvider)
    {
        var interfaceType = typeof(T);
        var types = AppDomain
            .CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type is { Namespace: not null })
            .Where(type => type.Namespace!.StartsWith("Radon"))
            .Where(type => interfaceType.IsAssignableFrom(type))
            .Where(type => type is { IsAbstract: false, IsClass: true });

        foreach (var type in types)
        {
            yield return (T)serviceProvider.GetService(type)!;
        }
    }
}
