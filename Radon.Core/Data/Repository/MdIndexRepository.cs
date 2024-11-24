using Radon.Core.Data.Entity;
using Radon.Data.Repository;

namespace Radon.Core.Data.Repository;

public class MdIndexRepository(IFreeSql fsql) : BaseRepository<MdIndex, Guid>(fsql)
{
    public MdIndex? FindByPath(string path) =>
        Select.Where(x => x.Path == Guid.Parse(path)).First();
}
