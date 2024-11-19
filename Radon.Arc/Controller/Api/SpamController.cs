using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Arc.Controller;

[ApiController, Route("api/spam")]
public class SpamController(ISpamService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(SpamRes), StatusCodes.Status200OK)]
    public IActionResult Fetch(SpamFetchReq req) => Ok(service.Fetch(req));

    [HttpPost("precise")]
    [ProducesResponseType(typeof(SpamRes), StatusCodes.Status200OK)]
    public IActionResult Precise(SpamFetchPreciseReq req) => Ok(service.FetchPrecise(req));

    [Authorize]
    [HttpPut]
    [ProducesResponseType(typeof(StateRes), StatusCodes.Status200OK)]
    public IActionResult Append(SpamAppendReq req) => Ok(new StateRes(service.Append(req)));
}
