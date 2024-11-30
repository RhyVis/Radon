using Radon.Core.Model.Base;

namespace Radon.Core.Model.Request;

public class TextStoreReq : BaseApiReq<TextStoreReq.ReqData>
{
    public record ReqData
    {
        public long Id { get; set; }
        public string Text { get; set; } = "";
        public string Note { get; set; } = "";
    }
}