using FreeSql.DataAnnotations;
using Radon.Data.Entity;

namespace Radon.Core.Data.Entity;

[Table(Name = "md_index")]
public class MdIndex : BaseEntity
{
    [Column(IsPrimary = true)]
    public Guid Path { get; set; }

    [Column(StringLength = 64, IsNullable = false)]
    public string Name { get; set; } = string.Empty;

    [Column(StringLength = 128, IsNullable = true)]
    public string Desc { get; set; } = string.Empty;

    [Column(ServerTime = DateTimeKind.Utc, CanUpdate = false)]
    public DateTime CreateTime { get; set; }

    [Column(ServerTime = DateTimeKind.Utc)]
    public DateTime UpdateTime { get; set; }
}
