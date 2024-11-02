using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

public class EscapeRes(string data) : BaseApiRes<string>(data)
{
    public EscapeRes(int code, string message, string data)
        : this(data)
    {
        Code = code;
        Msg = message;
    }
}
