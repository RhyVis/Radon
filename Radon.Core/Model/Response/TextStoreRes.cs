using Radon.Common.Core.Extension;
using Radon.Common.Enums;
using Radon.Core.Data.Entity;
using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

public class TextStoreRes : BaseApiRes<TextStoreRes.ResData>
{
    public static TextStoreRes FromEntity(EntryTextStorage entity)
    {
        return new TextStoreRes
        {
            Code =
                entity.Id < 0
                    ? ResponseCodeType.NOT_FOUND.ToInt()
                    : ResponseCodeType.SUCCESS.ToInt(),
            Data = new ResData
            {
                Id = entity.Id,
                Text = entity.Text,
                Note = entity.Note
            }
        };
    }

    public class ResData
    {
        public long Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }
}