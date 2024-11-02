using Microsoft.AspNetCore.Mvc;
using Radon.Common.Core.Config;

namespace Radon.Web.Controller;

[ApiController]
[Route("api")]
public class ApiRootController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(object), 200)]
    public IActionResult Get() => Ok(new { AppSettings.Get().Name, AppSettings.Get().Version });
}
