using Radon.Data.Entity.Base;

namespace Radon.Data.Repository.Base;

public class BaseRepository<E, K>(IFreeSql fsql) : FreeSql.BaseRepository<E, K>(fsql, null)
    where E : BaseEntity
    where K : struct
{
    public E? FindRand() => Select.OrderByRandom().Limit(1).First();

    public List<E> FindRand(int count) => Select.OrderByRandom().Limit(count).ToList();

    /// <summary>
    /// Be cautious when using this method, it will return all data from the table.
    /// </summary>
    public List<E> FindAll() => Select.ToList();
}
