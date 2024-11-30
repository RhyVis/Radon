using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

public class PlainNumberRes : BaseApiRes<long>
{
    private PlainNumberRes(long data)
        : base(data)
    {
    }

    public static PlainNumberRes Of(long data)
    {
        return new PlainNumberRes(data);
    }
}