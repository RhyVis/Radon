using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Arc.Controller;

[ApiController]
[Route("api/escape")]
public class EscapeController(IEscapeService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(EscapeRes), 200)]
    public IActionResult Get() => Ok(EscapeRes.Dummy());

    [HttpPost]
    [ProducesResponseType(typeof(EscapeRes), 200)]
    public IActionResult Post(EscapeReq req) => Ok(service.HandleEscape(req));
}
