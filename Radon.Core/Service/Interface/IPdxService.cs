namespace Radon.Core.Service.Interface;

public interface IPdxService
{
    string GetLangParserReplacer(long userId);
    void SetLangParserReplacer(long userId, string jsonData);
}