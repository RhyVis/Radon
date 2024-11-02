using FreeSql.DataAnnotations;
using Radon.Data.Entity.Base;

namespace Radon.Data.Entity.Core;

[Table(Name = "storage_text")]
public class EntryTextStorage : BaseEntity
{
    [Column(IsPrimary = true)]
    public long Id { get; set; }

    [Column(StringLength = -1)]
    public string Text { get; set; } = "";

    [Column(StringLength = 30)]
    public string Note { get; set; } = "";
}
