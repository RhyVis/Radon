using Radon.Core.Data.Entity;
using Radon.Data.Repository;

namespace Radon.Core.Data.Repository;

public class NavigationRepository(IFreeSql fsql) : BaseRepository<EntryNavigation, long>(fsql);