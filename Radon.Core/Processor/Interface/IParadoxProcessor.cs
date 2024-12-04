using Radon.Core.Util;

namespace Radon.Core.Processor.Interface;

public interface IParadoxProcessor
{
    List<PdxLangParsedItem> ParseLang(string paradoxYaml);

    List<PdxLangParsedItem> ParseLang(Stream inputStream);

    List<PdxLangParsedItem> ParseLangFromTextStorage(long textStorageId);

    List<PdxLangEventItem> ParseEvent(string paradoxYaml);

    List<PdxLangEventItem> ParseEventFromTextStorage(long textStorageId);
}
