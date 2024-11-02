using Radon.Core.Model.Base;

namespace Radon.Core.Model.Request;

public class TarotReq : BaseApiReq<TarotReq.TarotReqData>
{
    public class TarotReqData
    {
        public string Deck { get; set; } = "";
        public bool Full { get; set; }
        public int Size { get; set; }
    }
}
