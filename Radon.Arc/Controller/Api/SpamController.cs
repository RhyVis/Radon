using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Arc.Controller.Api;

[ApiController, Route("api/spam")]
public class SpamController(ISpamService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<SpamRes>(StatusCodes.Status200OK)]
    public IActionResult Fetch(SpamFetchReq req) => Ok(service.Fetch(req));

    [HttpPost("precise")]
    [ProducesResponseType<SpamRes>(StatusCodes.Status200OK)]
    public IActionResult Precise(SpamFetchPreciseReq req) => Ok(service.FetchPrecise(req));

    [Authorize]
    [HttpPut("append")]
    [ProducesResponseType<StateRes>(StatusCodes.Status200OK)]
    public IActionResult Append(SpamAppendReq req) => Ok(new StateRes(service.Append(req)));
}
