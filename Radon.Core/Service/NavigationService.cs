﻿using Radon.Common.Core.DI;
using Radon.Core.Data.Entity;
using Radon.Core.Data.Repository;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Core.Service;

[ServiceTransient]
public class NavigationService(NavigationRepository repo) : INavigationService
{
    public NavigationRes HandleNavigation() => new(GetNavigations().ToArray());

    private List<EntryNavigation> GetNavigations() => repo.FindAll();
}
