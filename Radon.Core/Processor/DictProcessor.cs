using Masuit.Tools;
using OpenCCNET;
using Radon.Common.Core.DI;
using Radon.Common.Core.Extension;
using Radon.Core.Enums;
using Radon.Core.Initializer;
using Radon.Core.Processor.Interface;

namespace Radon.Core.Processor;

[ServiceScoped]
public class DictProcessor : IDictProcessor
{
    public string ProcessDict(DictType type, string text, bool encode)
    {
        return type switch
        {
            DictType.NMSL => encode ? EncodeNmsl(text) : DecodeNmsl(text),
            DictType.TRAD => encode ? EncodeTrad(text) : DecodeTrad(text),
            DictType.SPRK => encode ? EncodeSprk(text) : DecodeSprk(text),
            DictType.DIFF => EncodeDiff(text),
            _ => text
        };
    }

    private static string EncodeNmsl(string text)
    {
        return text.SplitElement().Select(DictData.GetHanEmoji).Join("");
    }

    private static string DecodeNmsl(string text)
    {
        return text.SplitElement().Select(DictData.GetEmojiHan).Join("");
    }

    private static string EncodeSprk(string text)
    {
        return text.SplitElement().Select(DictData.GetHanSpark).Join("");
    }

    private static string DecodeSprk(string text)
    {
        return text.SplitElement().Select(DictData.GetSparkHan).Join("");
    }

    private static string EncodeDiff(string text)
    {
        return text.SplitElement().Select(DictData.GetUnicodeDiff).Join("");
    }

    private static string EncodeTrad(string text)
    {
        return ZhConverter.HansToHant(text);
    }

    private static string DecodeTrad(string text)
    {
        return ZhConverter.HantToHans(text);
    }
}