using Radon.Data.Entity;

namespace Radon.Data.Repository;

public abstract class BaseIdRepository<E, K>(IFreeSql fsql) : BaseRepository<E, K>(fsql)
    where E : BaseIdEntity
    where K : struct { }
