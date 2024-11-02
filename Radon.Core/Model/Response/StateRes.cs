using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

/// <summary>
/// The data is 0 or -1, 0 means success, -1 means failure.
/// </summary>
public class StateRes : BaseApiRes<int>
{
    public StateRes(bool success)
        : base(success ? 0 : -1) { }

    public StateRes()
        : base(0) { }
}
