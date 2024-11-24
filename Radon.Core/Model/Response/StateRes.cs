using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

/// <summary>
/// The data is true or false, true means success, false means failure.
/// </summary>
public class StateRes : BaseApiRes<bool>
{
    public StateRes(bool success)
        : base(success) { }

    public StateRes()
        : base(true) { }

    public static StateRes Of(bool success) => new(success);

    public static StateRes OfSuccess() => new(true);
}
