using System.Reflection;
using Masuit.Tools;
using Masuit.Tools.Reflection;

namespace Radon.Common.Core.Extension;

public static class NullCheckerExtension
{
    public static bool HasNullProperties(this object obj)
    {
        ArgumentNullException.ThrowIfNull(nameof(obj));

        return obj.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Any(property => property.GetValue(obj) == null);
    }
}
