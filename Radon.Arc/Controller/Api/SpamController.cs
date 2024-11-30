using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Arc.Controller.Api;

[ApiController]
[Route("api/spam")]
public class SpamController(ISpamService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<SpamRes>(StatusCodes.Status200OK)]
    public IActionResult Fetch(SpamFetchReq req)
    {
        return Ok(service.Fetch(req));
    }

    [HttpPost("precise")]
    [ProducesResponseType<SpamRes>(StatusCodes.Status200OK)]
    public IActionResult Precise(SpamFetchPreciseReq req)
    {
        return Ok(service.FetchPrecise(req));
    }

    [Authorize]
    [HttpPut("append")]
    [ProducesResponseType<PlainNumberRes>(StatusCodes.Status200OK)]
    public IActionResult Append(SpamAppendReq req)
    {
        return Ok(PlainNumberRes.Of(service.Append(req)));
    }
}