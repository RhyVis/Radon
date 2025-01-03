﻿using FreeSql.DataAnnotations;
using JetBrains.Annotations;
using Radon.Data.Entity;

namespace Radon.Core.Data.Entity;

[Table(Name = "md_index")]
public class MdIndex : BaseEntity
{
    [Column(IsPrimary = true)] public Guid Path { get; set; }

    [Column(StringLength = 64, IsNullable = false)]
    public string Name { get; set; } = string.Empty;

    [Column(StringLength = 128, IsNullable = true)]
    public string Desc { get; set; } = string.Empty;

    [Column(StringLength = -1, IsNullable = false)]
    public string Content { get; set; } = string.Empty;

    [Column(ServerTime = DateTimeKind.Utc, CanUpdate = false)]
    public DateTime CreateTime { get; set; }

    [Column(ServerTime = DateTimeKind.Utc)]
    public DateTime UpdateTime { get; set; }

    public MdIndexFullDto ToFullDto()
    {
        return new MdIndexFullDto
        {
            Path = Path,
            Name = Name,
            Desc = Desc,
            Content = Content,
            CreateTime = CreateTime,
            UpdateTime = UpdateTime
        };
    }
}

[UsedImplicitly(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.WithMembers)]
public class MdIndexDto
{
    public Guid Path { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}

[UsedImplicitly(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.WithMembers)]
public class MdIndexFullDto : MdIndexDto
{
    public string Content { get; set; } = string.Empty;
}
