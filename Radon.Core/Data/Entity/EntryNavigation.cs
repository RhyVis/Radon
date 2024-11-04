using FreeSql.DataAnnotations;
using Radon.Data.Entity;

namespace Radon.Core.Data.Entity;

[Table(Name = "storage_nav")]
public class EntryNavigation : BaseEntity
{
    [Column(IsPrimary = true)]
    public long Id { get; set; }

    [Column(StringLength = 256)]
    public string Data { get; set; } = "";

    [Column(StringLength = 32)]
    public string Label { get; set; } = "";

    [Column(StringLength = 32)]
    public string Note { get; set; } = "";
}
