using Radon.Core.Model.Base;

namespace Radon.Core.Model.Request;

public class SpamAppendReq : BaseApiReq<SpamAppendReq.SpamAppendData>
{
    public class SpamAppendData
    {
        public string Type { get; set; } = "";
        public string Text { get; set; } = "";
    }
}
