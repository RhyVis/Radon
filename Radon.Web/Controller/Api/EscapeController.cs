using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Web.Controller;

[ApiController]
[Route("api/escape")]
public class EscapeController(IEscapeService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(EscapeRes), 200)]
    public IActionResult Get() => Ok(new EscapeRes(666, "666", "玩抽象的这辈子有了"));

    [HttpPost]
    [ProducesResponseType(typeof(EscapeRes), 200)]
    public IActionResult Post(EscapeReq req) => Ok(service.HandleEscape(req));
}
