using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Arc.Controller.Api;

[ApiController, Route("api/escape")]
public class EscapeController(IEscapeService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<EscapeRes>(200)]
    public IActionResult Get() => Ok(EscapeRes.Dummy());

    [HttpPost]
    [ProducesResponseType<EscapeRes>(200)]
    public IActionResult Post(EscapeReq req) => Ok(service.HandleEscape(req));
}
