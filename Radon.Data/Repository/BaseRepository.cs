using JetBrains.Annotations;
using Radon.Data.Entity;

namespace Radon.Data.Repository;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public class BaseRepository<TE, TK>(IFreeSql fsql) : FreeSql.BaseRepository<TE, TK>(fsql, null)
    where TE : BaseEntity
    where TK : struct
{
    public TE? FindRand()
    {
        return Select.OrderByRandom().Limit(1).First();
    }

    public List<TE> FindRand(int count)
    {
        return Select.OrderByRandom().Limit(count).ToList();
    }

    /// <summary>
    ///     Be cautious when using this method, it will return all data from the table.
    /// </summary>
    public List<TE> FindAll()
    {
        return Select.ToList();
    }
}