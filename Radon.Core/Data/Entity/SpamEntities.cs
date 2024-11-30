using FreeSql.DataAnnotations;
using Radon.Data.Entity;

namespace Radon.Core.Data.Entity;

/// <summary>
///     Basic entity for SpamService.
/// </summary>
[Table(DisableSyncStructure = true)]
public abstract class SpamEntity : BaseIdEntity
{
    [Column(StringLength = -1, IsNullable = false)]
    public string Text { get; set; } = null!;

    public static DummySpamEntity Dummy()
    {
        return new DummySpamEntity();
    }
}

[Table(DisableSyncStructure = true)]
public class DummySpamEntity : SpamEntity
{
    public DummySpamEntity()
    {
        Id = -1;
        Text = "No data found";
    }
}

[Table(Name = "gacha_ak")]
public class GachaAk : SpamEntity;

[Table(Name = "gacha_gs")]
public class GachaGs : SpamEntity;

[Table(Name = "gacha_ml")]
public class GachaMl : SpamEntity;

[Table(Name = "meme_acgn")]
public class MemeAcgn : SpamEntity;

[Table(Name = "meme_dinner")]
public class MemeDinner : SpamEntity;

[Table(Name = "spam_max")]
public class SpamMax : SpamEntity;

[Table(Name = "spam_min")]
public class SpamMin : SpamEntity;