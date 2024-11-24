using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Arc.Controller.Api;

[Authorize]
[ApiController, Route("api/nav")]
public class NavigationController(INavigationService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<NavigationRes>(StatusCodes.Status200OK)]
    public IActionResult GetNavigation() => Ok(service.HandleNavigation());
}
