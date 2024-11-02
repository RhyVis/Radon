using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

public class TextStoreRes : BaseApiRes<TextStoreRes.TextStoreResData>
{
    public class TextStoreResData
    {
        public long Id { get; set; }
        public string Text { get; set; } = "";
        public string Note { get; set; } = "";
    }
}
