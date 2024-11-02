using Radon.Data.Entity.Core;
using Radon.Data.Repository.Base;

namespace Radon.Data.Repository.Core;

public class NavigationRepository(IFreeSql fsql) : BaseRepository<EntryNavigation, long>(fsql) { }
