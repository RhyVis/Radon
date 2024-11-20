using Radon.Core.Util;

namespace Radon.Core.Processor.Interface;

public interface IParadoxProcessor
{
    [Obsolete("Not useful in server")]
    List<PdxLangParsedItem> ParseLang(string paradoxYamlPath);

    List<PdxLangParsedItem> ParseLang(Stream inputStream);
}
