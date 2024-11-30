using Radon.Core.Model.Base;

namespace Radon.Core.Model.Request;

public class MdReq : BaseApiReq<MdReq.ReqData>
{
    public record ReqData
    {
        public string Path { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Desc { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
    }
}