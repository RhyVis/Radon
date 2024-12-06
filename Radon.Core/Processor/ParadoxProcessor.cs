using NLog;
using Radon.Common.Core.DI;
using Radon.Core.Data.Repository;
using Radon.Core.Processor.Interface;
using Radon.Core.Util;

namespace Radon.Core.Processor;

[ServiceScoped]
public class ParadoxProcessor(TextStorageRepository repo) : IParadoxProcessor
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public List<PdxLangParsedItem> ParseLang(string paradoxYaml)
    {
        var parser = PdxLangParser.Create(paradoxYaml);
        return parser.GetParsedItems();
    }

    public List<PdxLangParsedItem> ParseLang(Stream inputStream)
    {
        var parser = PdxLangParser.Create(inputStream);
        return parser.GetParsedItems();
    }

    public List<PdxLangParsedItem> ParseLangFromTextStorage(long textStorageId)
    {
        var content = repo.Select
            .Where(e => e.Id == textStorageId)
            .ToOne(e => e.Text);
        if (content is null) return [];
        Logger.Info($"Found from text storage by: {textStorageId}");
        return ParseLang(content);
    }

    public List<PdxLangEventItem> ParseEvent(string paradoxYaml)
    {
        var parser = PdxLangParser.Create(paradoxYaml);
        return parser.GetEventItems();
    }

    public List<PdxLangEventItem> ParseEventFromTextStorage(long textStorageId)
    {
        var content = repo.Select
            .Where(e => e.Id == textStorageId)
            .ToOne(e => e.Text);
        if (content is null) return [];
        Logger.Info($"Found from text storage by: {textStorageId}");
        return ParseEvent(content);
    }
}
