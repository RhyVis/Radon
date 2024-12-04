using Radon.Common.Core.DI;
using Radon.Core.Data.Repository;
using Radon.Core.Processor.Interface;
using Radon.Core.Util;

namespace Radon.Core.Processor;

[ServiceScoped]
public class ParadoxProcessor(TextStorageRepository repo) : IParadoxProcessor
{
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
        var content = repo.Select.Where(e => e.Id == textStorageId).First()?.Text;
        return content is null ? [] : ParseLang(content);
    }

    public List<PdxLangEventItem> ParseEvent(string paradoxYaml)
    {
        var parser = PdxLangParser.Create(paradoxYaml);
        return parser.GetEventItems();
    }

    public List<PdxLangEventItem> ParseEventFromTextStorage(long textStorageId)
    {
        var content = repo.Select.Where(e => e.Id == textStorageId).First()?.Text;
        return content is null ? [] : ParseEvent(content);
    }
}
