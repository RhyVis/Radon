using Radon.Data.Entity.Core;
using Radon.Data.Repository.Base;

namespace Radon.Data.Repository.Core
{
    # region SpamRepository

    public class GachaAkRepository(IFreeSql fsql) : BaseIdRepository<GachaAk, long>(fsql) { }

    public class GachaGsRepository(IFreeSql fsql) : BaseIdRepository<GachaGs, long>(fsql) { }

    public class GachaMlRepository(IFreeSql fsql) : BaseIdRepository<GachaMl, long>(fsql) { }

    public class MemeAcgnRepository(IFreeSql fsql) : BaseIdRepository<MemeAcgn, long>(fsql) { }

    public class MemeDinnerRepository(IFreeSql fsql) : BaseIdRepository<MemeDinner, long>(fsql) { }

    public class SpamMaxRepository(IFreeSql fsql) : BaseIdRepository<SpamMax, long>(fsql) { }

    public class SpamMinRepository(IFreeSql fsql) : BaseIdRepository<SpamMin, long>(fsql) { }

    # endregion
}
