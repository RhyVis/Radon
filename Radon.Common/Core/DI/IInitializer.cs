using JetBrains.Annotations;

namespace Radon.Common.Core.DI;

/// <summary>
/// Services implementing this interface will be initialized after the application built, before the application runs.
/// <br/>
/// Services will be registered as Transient, disposed after the initialization. It should not be used after the initialization.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public interface IInitializer
{
    /// <summary>
    /// Async initialization method
    /// </summary>
    /// <returns></returns>
    Task<object> InitAsync();
}
