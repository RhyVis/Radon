using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Web.Controller;

[ApiController]
[Route("api/auth")]
public class SimpleAuthenticationController(ISimpleAuthenticationService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(StateRes), StatusCodes.Status200OK)]
    public IActionResult Handle(PlainTextReq req) => Ok(service.HandleSimpleAuthentication(req));
}
