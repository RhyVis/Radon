using Radon.Core.Model.Base;

namespace Radon.Core.Model.Request;

public class SpamFetchReq : BaseApiReq<SpamFetchReq.SpamReqData>
{
    public class SpamReqData
    {
        public string Type { get; set; } = "";
        public string Dict { get; set; } = "";
        public int Size { get; set; } = 1;
    }
}

public class SpamFetchPreciseReq : BaseApiReq<SpamFetchPreciseReq.SpamPreciseReqData>
{
    public class SpamPreciseReqData
    {
        public string Type { get; set; } = "";
        public string Dict { get; set; } = "";
        public long[] Ids { get; set; } = [];
    }
}
