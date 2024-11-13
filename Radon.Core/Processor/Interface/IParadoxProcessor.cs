using Radon.Core.Util;

namespace Radon.Core.Processor.Interface;

public interface IParadoxProcessor
{
    List<PdxLangParsedItem> ParseLang(string paradoxYamlPath);

    List<PdxLangParsedItem> ParseLang(Stream inputStream);
}
