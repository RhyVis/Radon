using Radon.Common.Core.DI;
using Radon.Core.Processor.Interface;
using Radon.Core.Util;

namespace Radon.Core.Processor;

[ServiceScoped]
public class ParadoxProcessor : IParadoxProcessor
{
    public List<PdxLangParsedItem> ParseLang(string paradoxYamlPath)
    {
        var parser = PdxLangParser.Create(paradoxYamlPath);
        return parser.Parse();
    }

    public List<PdxLangParsedItem> ParseLang(Stream inputStream)
    {
        var parser = PdxLangParser.Create(inputStream);
        return parser.Parse();
    }
}