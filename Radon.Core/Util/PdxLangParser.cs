using System.Text.RegularExpressions;
using Masuit.Tools;
using NLog;
using Radon.Core.Exceptions;

namespace Radon.Core.Util;

public partial class PdxLangParser
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly string[] _lines;
    private readonly List<PdxLangParsedItem> _parsedItems;

    private PdxLangParser(string[] lines)
    {
        _lines = lines;
        _parsedItems = Parse();
    }

    private PdxLangParser(string content)
    {
        try
        {
            _lines = content.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            _parsedItems = Parse();
        }
        catch (Exception e)
        {
            Logger.Error(e, "Failed to parse as PdxLang");
            throw new ServiceFailureException("Failed to parse as PdxLang", e);
        }
    }

    private PdxLangParser(Stream inputStream)
    {
        try
        {
            using var reader = new StreamReader(inputStream);
            var lines = new List<string>();
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null) break;

                lines.Add(line);
            }

            _lines = lines.ToArray();
            _parsedItems = Parse();
        }
        catch (Exception e)
        {
            Logger.Error(e, "Failed to read PdxLang file");
            throw new ServiceFailureException("Failed to read Pdx Lang file", e);
        }
    }

    [GeneratedRegex("^(name|desc|[a-z])$")]
    private partial Regex EventSpec();

    private List<PdxLangParsedItem> Parse()
    {
        Logger.Info($"Reading PdxLang for {_lines[0].Trim().TrimEnd(':')}");
        return _lines
            .Skip(1)
            .Where(l => !(string.IsNullOrWhiteSpace(l) || l.TrimStart().StartsWith('#')))
            .Select(l => l.Split(':', 2))
            .Where(pts => pts.Length == 2)
            .Select(pts => new PdxLangParsedItem
            {
                Name = pts[0].Trim(),
                Namespace = pts[0].Trim().Split('.'),
                Value = pts[1].Trim().TrimStart('0').Trim().Trim('"')
            })
            .Where(x => x.Namespace.Length >= 1)
            .ToList();
    }

    public List<PdxLangParsedItem> GetParsedItems()
    {
        return _parsedItems.ToList();
    }

    public List<PdxLangEventItem> GetEventItems()
    {
        var groupedItems = _parsedItems.Where(item => EventSpec().IsMatch(item.Namespace.Last()))
            .GroupBy(item => item.Namespace.Take(item.Namespace.Length - 1).Join("/"))
            .Where(group => group.Any(item => item.Namespace.Last() is "name"))
            .Select(group => new PdxLangEventItem
            {
                Name = group.FirstOrDefault(item => item.Namespace.Last() == "name")?.Value ?? string.Empty,
                Desc = group.FirstOrDefault(item => item.Namespace.Last() == "desc")?.Value ?? string.Empty,
                Options = group.Where(item => item.Namespace.Last() is not "name" and not "desc")
                    .Select(item => new PdxLangEventItem.Option(item.Namespace.Last(), item.Value)).ToArray()
            });
        return groupedItems.ToList();
    }

    // Unchecked
    public static PdxLangParser Create(string[] lines)
    {
        return new PdxLangParser(lines);
    }

    public static PdxLangParser Create(string content)
    {
        return new PdxLangParser(content);
    }

    public static PdxLangParser Create(Stream inputStream)
    {
        return new PdxLangParser(inputStream);
    }
}

public class PdxLangParsedItem
{
    public string Name { get; init; } = string.Empty;
    public string[] Namespace { get; init; } = [];
    public string Value { get; init; } = string.Empty;

    public override string ToString()
    {
        return $"{string.Join('/', Namespace)}->{Value}";
    }
}

public class PdxLangEventItem
{
    public string Name { get; init; } = string.Empty;
    public string Desc { get; init; } = string.Empty;
    public Option[] Options { get; init; } = [];

    public record Option(string Key, string Name);
}
