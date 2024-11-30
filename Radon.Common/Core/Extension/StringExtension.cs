using System.Globalization;

namespace Radon.Common.Core.Extension;

public static class StringExtension
{
    /// <summary>
    ///     Split string by element, to support some special characters like emoji.
    /// </summary>
    /// <param name="s">Original string</param>
    /// <returns>Each Element</returns>
    public static IEnumerable<string> SplitElement(this string s)
    {
        var stringInfo = new StringInfo(s);
        for (var i = 0; i < stringInfo.LengthInTextElements; i++) yield return stringInfo.SubstringByTextElements(i, 1);
    }
}