using System.Reflection;

namespace Radon.Common.Core.Extension;

public static class NullCheckerExtension
{
    public static bool HasNullProperties(this object? obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        return obj.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToList()
            .Exists(p => p.GetValue(obj) is null);
    }
}
