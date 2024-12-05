using Masuit.Tools;
using NLog;
using Radon.Core.Exceptions;

namespace Radon.Core.Util;

public class PdxLangParser
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
        var prefixes = _parsedItems
            .Where(item => item.Namespace.Last() is "name" && item.Namespace.Length > 1)
            .Select(item => item.Namespace.Take(item.Namespace.Length - 1).Join("/"))
            .Distinct()
            .Where(prefix => _parsedItems.Any(item =>
                item.Namespace.Take(prefix.Split('/').Length).Join("/") == prefix &&
                (item.Namespace.Last().StartsWith("desc") || item.Namespace[^2] is "desc")
            ))
            .ToList();

        var groups = prefixes
            .Select(prefix => new
            {
                Key = prefix,
                Items = _parsedItems
                    .Where(item => item.Namespace.Take(prefix.Split('/').Length).Join("/") == prefix)
            });

        var events = groups
            .Select(group =>
            {
                var contentItems = group.Items
                    .Where(item =>
                        item.Namespace.Last() != "name" &&
                        !item.Namespace.Last().StartsWith("desc") &&
                        item.Namespace[^2] != "desc")
                    .ToArray();
                var respItems = contentItems
                    .Where(item => item.Namespace.Last().StartsWith("resp"))
                    .ToArray();
                var respItemInfo = respItems
                    .Select(item => new
                    {
                        Ref = item.Namespace[^2],
                        Content = item.Value
                    }).ToArray();
                var respItemLookup = respItemInfo
                    .Select(info => info.Ref)
                    .ToArray();
                var respItemRefs = contentItems
                    .Where(item => respItemLookup.Contains(item.Namespace.Last()))
                    .ToArray();

                var optionsWithResp = respItemRefs
                    .Select(item => new PdxLangEventItem.Option(
                        item.Namespace.Last(),
                        item.Value,
                        respItemInfo.First(info => info.Ref == item.Namespace.Last()).Content
                    ));
                var optionsOther = contentItems
                    .Where(item => !respItems.Contains(item) && !respItemRefs.Contains(item))
                    .Select(item => new PdxLangEventItem.Option(
                        item.Namespace.Last(), item.Value
                    ));

                return new PdxLangEventItem
                {
                    Key = group.Key,
                    Name = group.Items.FirstOrDefault(item => item.Namespace.Last() == "name")?.Value ?? string.Empty,
                    Desc = group.Items
                        .Where(i => i.Namespace.Last().StartsWith("desc") || i.Namespace[^2] is "desc")
                        .Select(i => i.Value)
                        .Join("\n"),
                    Options = optionsOther.Concat(optionsWithResp).ToArray()
                };
            });

        return events.ToList();
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
    public string Key { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Desc { get; init; } = string.Empty;
    public Option[] Options { get; init; } = [];

    public record Option(string Key, string Name = "", string Resp = "");
}
