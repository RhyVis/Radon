using Radon.Core.Data.Entity;
using Radon.Data.Repository;

namespace Radon.Core.Data.Repository;

# region SpamRepository

public class GachaAkRepository(IFreeSql fsql) : BaseIdRepository<GachaAk>(fsql) { }

public class GachaGsRepository(IFreeSql fsql) : BaseIdRepository<GachaGs>(fsql) { }

public class GachaMlRepository(IFreeSql fsql) : BaseIdRepository<GachaMl>(fsql) { }

public class MemeAcgnRepository(IFreeSql fsql) : BaseIdRepository<MemeAcgn>(fsql) { }

public class MemeDinnerRepository(IFreeSql fsql) : BaseIdRepository<MemeDinner>(fsql) { }

public class SpamMaxRepository(IFreeSql fsql) : BaseIdRepository<SpamMax>(fsql) { }

public class SpamMinRepository(IFreeSql fsql) : BaseIdRepository<SpamMin>(fsql) { }

# endregion
