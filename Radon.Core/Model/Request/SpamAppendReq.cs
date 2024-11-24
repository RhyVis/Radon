using Radon.Core.Model.Base;

namespace Radon.Core.Model.Request;

public class SpamAppendReq : BaseApiReq<SpamAppendReq.ReqData>
{
    public record ReqData
    {
        public string Type { get; set; } = "";
        public string Text { get; set; } = "";
    }
}
