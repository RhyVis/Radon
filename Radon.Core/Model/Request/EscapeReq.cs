using Radon.Core.Model.Base;

namespace Radon.Core.Model.Request;

public class EscapeReq : BaseApiReq<EscapeReq.EscapeReqData>
{
    public class EscapeReqData
    {
        public string Type { get; set; } = null!;
        public string Text { get; set; } = null!;
        public bool Encode { get; set; }
    }
}
