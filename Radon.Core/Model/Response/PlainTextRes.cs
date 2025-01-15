using Radon.Common.Core.Extension;
using Radon.Common.Enums;
using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

public class PlainTextRes : BaseApiRes<string>
{
    private PlainTextRes(string data)
        : base(data)
    {
    }

    private PlainTextRes(string msg, string data)
        : base(ResponseCodeType.Success.ToInt(), msg, data)
    {
    }

    public static PlainTextRes Of(string data)
    {
        return new PlainTextRes(data);
    }

    public static PlainTextRes Of(string msg, string data)
    {
        return new PlainTextRes(msg, data);
    }
}