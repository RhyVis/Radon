using FreeSql.DataAnnotations;

namespace Radon.Data.Entity;

/// <summary>
///     Base entity for all entities, including nothing.
/// </summary>
[Table(DisableSyncStructure = true)]
public abstract class BaseEntity;

/// <summary>
///     Base entity for all entities with an identity for ID field also as Primary Key.
/// </summary>
[Table(DisableSyncStructure = true)]
public abstract class BaseIdEntity : BaseEntity
{
    [Column(IsPrimary = true, IsIdentity = true)]
    public long Id { get; set; }
}
