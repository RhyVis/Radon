using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

public class EscapeRes : BaseApiRes<string>
{
    private EscapeRes(int code, string message, string data)
        : base(code, message, data)
    {
    }

    private EscapeRes(string data)
        : base(data)
    {
    }

    public static EscapeRes Of(string data)
    {
        return new EscapeRes(data);
    }

    public static EscapeRes Of(int code, string message, string data)
    {
        return new EscapeRes(code, message, data);
    }

    public static EscapeRes Invalid(string data)
    {
        return new EscapeRes(-1, "Invalid dict type", data);
    }

    public static EscapeRes Dummy()
    {
        return new EscapeRes(666, "666", "玩抽象的这辈子有了");
    }
}