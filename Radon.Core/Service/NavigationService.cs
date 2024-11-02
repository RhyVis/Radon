using Radon.Common.Core.DI;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;
using Radon.Data.Entity.Core;
using Radon.Data.Repository.Core;

namespace Radon.Core.Service;

[ServiceTransient]
public class NavigationService(NavigationRepository repo) : INavigationService
{
    public NavigationRes HandleNavigation() => new(GetNavigations().ToArray());

    private List<EntryNavigation> GetNavigations() => repo.FindAll();
}
