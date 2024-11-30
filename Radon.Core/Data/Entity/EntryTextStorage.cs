using FreeSql.DataAnnotations;
using Radon.Data.Entity;

namespace Radon.Core.Data.Entity;

[Table(Name = "storage_text")]
public class EntryTextStorage : BaseEntity
{
    [Column(IsPrimary = true)] public long Id { get; init; }

    [Column(StringLength = -1)] public string Text { get; set; } = "";

    [Column(StringLength = 30)] public string Note { get; set; } = "";
}