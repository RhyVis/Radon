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
            .Select(pts =>
            {
                var ptKey = pts[0].Trim();
                var ptVal = pts[1].Trim().TrimStart('0').Trim();

                if (ptVal.StartsWith('\"')) ptVal = ptVal[1..];
                if (ptVal.EndsWith('\"')) ptVal = ptVal[..^1];

                return new PdxLangParsedItem
                {
                    Name = ptKey,
                    Namespace = ptKey.Split('.'),
                    Value = ptVal
                };
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
        // Find Namespace Prefixes for Event Items
        var prefixes = _parsedItems
            // Find all items with name or desc as last namespace, and more than 1 namespace len
            .Where(item => item.Namespace.Last() is "name" or "desc" && item.Namespace.Length > 1)
            .Select(item => item.Namespace
                .Take(item.Namespace.Length - 1)
                .Join("/"))
            .Distinct()
            .Select(prefix => new
            {
                Len = prefix.Split('/').Length,
                Pfx = prefix
            })
            .Where(prefix => _parsedItems.Any(item =>
                item.Namespace
                    .Take(prefix.Len)
                    .Join("/") == prefix.Pfx))
            .ToHashSet();

        var events = prefixes
            // Create Namespace Group
            .Select(prefix => new
            {
                Key = prefix.Pfx,
                Items = _parsedItems
                    .Where(item => item.Namespace.Take(prefix.Len).Join("/") == prefix.Pfx)
            })
            // Each Group: Create Event Item
            .Select(group =>
            {
                var contentItems = group.Items
                    .Where(item =>
                        item.Namespace.Last() != "name" &&
                        !item.Namespace.Last().StartsWith("desc") &&
                        item.Namespace[^2] != "desc")
                    .ToHashSet();

                var respItems = contentItems
                    .Where(item => item.Namespace.Last().StartsWith("resp"))
                    .ToHashSet();
                var respItemInfo = respItems
                    .Select(item => new
                    {
                        Ref = item.Namespace[^2],
                        Content = item.Value
                    }).ToHashSet();
                var respItemLookup = respItemInfo
                    .Select(info => info.Ref)
                    .ToHashSet();
                var respItemRefs = contentItems
                    .Where(item => respItemLookup.Contains(item.Namespace.Last()))
                    .ToHashSet();

                var tooltipItems = contentItems
                    .Where(item => item.Namespace.Last().StartsWith("tooltip"))
                    .ToHashSet();
                var tooltipItemInfo = tooltipItems
                    .Select(item => new
                    {
                        Ref = item.Namespace[^2],
                        Content = item.Value
                    }).ToHashSet();
                var tooltipItemLookup = tooltipItemInfo
                    .Select(info => info.Ref)
                    .ToHashSet();
                var tooltipItemRefs = contentItems
                    .Where(item => tooltipItemLookup.Contains(item.Namespace.Last()))
                    .ToHashSet();

                var optionsWithRespAndTooltip = respItemRefs
                    .Where(item => tooltipItemLookup.Contains(item.Namespace.Last()))
                    .Select(item =>
                    {
                        var last = item.Namespace.Last();
                        return new PdxLangEventItem.Option(
                            last,
                            item.Value,
                            respItemInfo.First(info => info.Ref == last).Content,
                            tooltipItemInfo.First(info => info.Ref == last).Content
                        );
                    });

                var optionsWithResp = respItemRefs
                    .Where(item => !tooltipItemLookup.Contains(item.Namespace.Last()))
                    .Select(item => new PdxLangEventItem.Option(
                        item.Namespace.Last(),
                        item.Value,
                        respItemInfo.First(info => info.Ref == item.Namespace.Last()).Content
                    ));

                var optionsWithTooltip = tooltipItemRefs
                    .Where(item => !respItemLookup.Contains(item.Namespace.Last()))
                    .Select(item => new PdxLangEventItem.Option(
                        item.Namespace.Last(),
                        item.Value,
                        "",
                        tooltipItemInfo.First(info => info.Ref == item.Namespace.Last()).Content
                    ));

                var optionsOther = contentItems
                    .Except(respItems)
                    .Except(respItemRefs)
                    .Except(tooltipItems)
                    .Except(tooltipItemRefs)
                    .Select(item => new PdxLangEventItem.Option(
                        item.Namespace.Last(), item.Value
                    ));

                var desc = group.Items
                    .Any(item => item.Namespace.Last().StartsWith("desc"))
                    ? group.Items
                        .Where(i => i.Namespace.Last().StartsWith("desc") || i.Namespace[^2] is "desc")
                        .Select(i => i.Value)
                        .Join("\n")
                    : group.Items
                        .Where(item => item.Namespace[^2] is "desc")
                        .Select(item => new[] { item.Namespace.Last().ToUpper(), item.Value })
                        .Select(arr => arr.Join("\n"))
                        .Join("\n");

                var options = optionsOther
                    .Concat(optionsWithRespAndTooltip)
                    .Concat(optionsWithResp)
                    .Concat(optionsWithTooltip)
                    .OrderBy(opt => opt.Key)
                    .ToArray();

                return new PdxLangEventItem
                {
                    Key = group.Key,
                    Name = group.Items.FirstOrDefault(item => item.Namespace.Last() == "name")?.Value ?? string.Empty,
                    Desc = desc,
                    Options = options
                };
            });

        return events
            .OrderBy(e => e.Key)
            .ToList();
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

    public override string ToString()
    {
        return
            $"{Key} - {Name} - {Desc} - {
                Options
                    .Select(opt =>
                        new[] { opt.Key, opt.Name, opt.Resp, opt.Tooltip, opt.ShowResp.ToString() }
                            .Select(arr => arr.Join("|"))
                    )
                    .Join()}";
    }

    public record Option(string Key, string Name = "", string Resp = "", string Tooltip = "", bool ShowResp = false);
}
