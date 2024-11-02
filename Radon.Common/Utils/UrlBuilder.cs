using System.Text;
using System.Text.RegularExpressions;

namespace Radon.Common.Utils;

public partial class UrlBuilder
{
    private const char SEP = '/';
    private StringBuilder _builder;

    private UrlBuilder(string baseUrl, bool defaultAppend = true)
    {
        _builder = new StringBuilder(baseUrl);
        if (defaultAppend)
        {
            _builder.Append(SEP);
        }
    }

    [GeneratedRegex("(?<!:)/{2,}")]
    private static partial Regex MultiSep();

    public UrlBuilder Append(string path)
    {
        _builder.Append(SEP).Append(path);
        return this;
    }

    public UrlBuilder Append(params string[] paths)
    {
        foreach (var path in paths)
        {
            _builder.Append(SEP).Append(path);
        }
        return this;
    }

    public string Build()
    {
        return MultiSep().Replace(_builder.ToString(), SEP.ToString());
    }

    public override string ToString()
    {
        return Build();
    }

    public static UrlBuilder Create(string baseUrl, bool defaultAppend = true)
    {
        return new UrlBuilder(baseUrl, defaultAppend);
    }
}
