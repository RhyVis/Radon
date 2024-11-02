using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Web.Controller;

[ApiController]
[Route("api/nav")]
public class NavigationController(INavigationService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(NavigationRes), 200)]
    public IActionResult GetNavigation() => Ok(service.HandleNavigation());
}
