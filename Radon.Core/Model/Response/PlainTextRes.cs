using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

public class PlainTextRes : BaseApiRes<string>
{
    private PlainTextRes(string data)
        : base(data) { }

    public static PlainTextRes Of(string data) => new(data);
}
