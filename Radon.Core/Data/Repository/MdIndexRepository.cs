using Radon.Core.Data.Entity;
using Radon.Data.Repository;

namespace Radon.Core.Data.Repository;

public class MdIndexRepository(IFreeSql fsql) : BaseRepository<MdIndex, Guid>(fsql)
{
    public MdIndex? FindByPath(string path)
    {
        var guid = Guid.Parse(path);
        return Select.Where(x => x.Path == guid).ToOne();
    }
}