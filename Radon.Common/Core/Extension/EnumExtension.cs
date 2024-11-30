using System.ComponentModel;
using Radon.Common.Core.Exception;

namespace Radon.Common.Core.Extension;

public static class EnumExtension
{
    public static int ToInt(this Enum e)
    {
        return e.GetHashCode();
    }

    /// <summary>
    ///     Parse a string to Enum, if failed return default value provided
    /// </summary>
    /// <param name="value">String presenting the name of enum</param>
    /// <param name="defaultValue">Default enum val, used as TEnum</param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns>The enum val or default</returns>
    public static TEnum TryParseEnum<TEnum>(this string value, TEnum defaultValue)
        where TEnum : struct, Enum
    {
        return Enum.TryParse(value, true, out TEnum result) ? result : defaultValue;
    }

    /// <summary>
    ///     Get Description attribute of Enum
    /// </summary>
    /// <param name="val">Enum</param>
    /// <returns>The Description or its name</returns>
    /// <exception cref="UnexpectedException">Type or Field of the Enum not found</exception>
    public static string GetDescription(this Enum val)
    {
        var type = val.GetType();
        var name = Enum.GetName(type, val) ?? throw new UnexpectedException();

        var field =
            type.GetField(name)
            ?? throw new UnexpectedException($"Cannot get field for enum {val}");

        if (
            Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
            is DescriptionAttribute attribute
        )
            return attribute.Description;

        return name;
    }
}
