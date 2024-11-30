using Radon.Core.Data.Entity;
using Radon.Data.Repository;

namespace Radon.Core.Data.Repository;

# region SpamRepository

public class BaseSpamRepository<TEntity> : BaseIdRepository<TEntity> where TEntity : SpamEntity
{
    protected BaseSpamRepository(IFreeSql fsql) : base(fsql)
    {
    }

    public bool CheckIfDuplicate(string text)
    {
        return Select.Where(x => x.Text.Trim() == text.Trim()).Any();
    }
}

public class GachaAkRepository(IFreeSql fsql) : BaseSpamRepository<GachaAk>(fsql);

public class GachaGsRepository(IFreeSql fsql) : BaseSpamRepository<GachaGs>(fsql);

public class GachaMlRepository(IFreeSql fsql) : BaseSpamRepository<GachaMl>(fsql);

public class MemeAcgnRepository(IFreeSql fsql) : BaseSpamRepository<MemeAcgn>(fsql);

public class MemeDinnerRepository(IFreeSql fsql) : BaseSpamRepository<MemeDinner>(fsql);

public class SpamMaxRepository(IFreeSql fsql) : BaseSpamRepository<SpamMax>(fsql);

public class SpamMinRepository(IFreeSql fsql) : BaseSpamRepository<SpamMin>(fsql);

# endregion