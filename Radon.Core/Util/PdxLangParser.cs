using NLog;
using Radon.Core.Exceptions;

namespace Radon.Core.Util;

public class PdxLangParser
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly string[] _lines;

    private PdxLangParser(string[] lines)
    {
        _lines = lines;
    }

    private PdxLangParser(string path)
    {
        try
        {
            _lines = File.ReadAllLines(path);
        }
        catch (Exception e)
        {
            Logger.Error(e, "Failed to read PdxLang file");
            throw new ServiceFailureException("Failed to read Pdx Lang file", e);
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
        }
        catch (Exception e)
        {
            Logger.Error(e, "Failed to read PdxLang file");
            throw new ServiceFailureException("Failed to read Pdx Lang file", e);
        }
    }

    public List<PdxLangParsedItem> Parse()
    {
        Logger.Info($"Reading PdxLang for {_lines[0].Trim().TrimEnd(':')}");
        return _lines
            .Skip(1)
            .Where(l => !(string.IsNullOrWhiteSpace(l) || l.TrimStart().StartsWith('#')))
            .Select(l => l.Split(':', 2))
            .Where(pts => pts.Length == 2)
            .Select(pts => new PdxLangParsedItem
            {
                Namespace = pts[0].Trim().Split('.'),
                Value = pts[1].Trim().TrimStart('0').Trim().Trim('"')
            })
            .Where(x => x.Namespace.Length >= 1)
            .ToList();
    }

    public static PdxLangParser Create(string[] lines)
    {
        return new PdxLangParser(lines);
    }

    public static PdxLangParser Create(string path)
    {
        return new PdxLangParser(path);
    }

    public static PdxLangParser Create(Stream inputStream)
    {
        return new PdxLangParser(inputStream);
    }
}

public class PdxLangParsedItem
{
    public string[] Namespace { get; init; } = [];
    public string Value { get; init; } = string.Empty;

    public override string ToString()
    {
        return $"{string.Join('/', Namespace)}->{Value}";
    }
}
