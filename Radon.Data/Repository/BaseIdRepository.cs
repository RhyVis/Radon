using Radon.Data.Entity;

namespace Radon.Data.Repository;

public abstract class BaseIdRepository<E>(IFreeSql fsql) : BaseRepository<E, long>(fsql)
    where E : BaseIdEntity
{
    public List<E> FindIn(IEnumerable<long> ids) => Select.Where(x => ids.Contains(x.Id)).ToList();
}
