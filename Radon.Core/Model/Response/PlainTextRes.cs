using Radon.Common.Core.Extension;
using Radon.Common.Enums;
using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

public class PlainTextRes : BaseApiRes<string>
{
    private PlainTextRes(string data)
        : base(data) { }

    private PlainTextRes(string msg, string data)
        : base(ResponseCodeType.SUCCESS.ToInt(), msg, data) { }

    public static PlainTextRes Of(string data) => new(data);

    public static PlainTextRes Of(string msg, string data) => new(msg, data);
}
