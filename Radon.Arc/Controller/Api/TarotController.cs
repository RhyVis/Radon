using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Arc.Controller;

[ApiController]
[Route("api/tarot")]
public class TarotController(ITarotService tarotService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(TarotRes), 200)]
    public IActionResult Get() => Ok(tarotService.HandleTarot());

    [HttpPost]
    [ProducesResponseType(typeof(TarotRes), 200)]
    public IActionResult Post(TarotReq req) => Ok(tarotService.HandleTarotComplex(req));

    [HttpGet("info")]
    [ProducesResponseType(typeof(UnsetRes), 200)]
    public IActionResult GetInfo() => Ok(tarotService.HandleTarotInfo());
}
