using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Web.Controller;

[ApiController]
[Route("api/spam")]
public class SpamController(ISpamService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(SpamRes), StatusCodes.Status200OK)]
    public IActionResult Post(SpamReq req) => Ok(service.HandleSpam(req));
}
