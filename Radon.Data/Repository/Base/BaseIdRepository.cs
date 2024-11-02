using Radon.Data.Entity.Base;

namespace Radon.Data.Repository.Base;

public abstract class BaseIdRepository<E, K>(IFreeSql fsql) : BaseRepository<E, K>(fsql)
    where E : BaseIdEntity
    where K : struct { }
