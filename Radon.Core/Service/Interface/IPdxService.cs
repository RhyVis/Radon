namespace Radon.Core.Service.Interface;

public interface IPdxService
{
    string GetLangParserReplacer(long userId);
    bool SetLangParserReplacer(long userId, string jsonData);
}
