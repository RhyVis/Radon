using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Web.Controller;

[ApiController]
[Route("api/save")]
public class SaveController(ITextStoreService service) : ControllerBase
{
    [HttpPost]
    [Route("query")]
    [ProducesResponseType(typeof(TextStoreRes), 200)]
    public IActionResult Query(TextStoreReq req) => Ok(service.HandleTextStoreQuery(req));

    [HttpPost]
    [Route("update")]
    [ProducesResponseType(typeof(StateRes), 200)]
    public IActionResult Update(TextStoreReq req) => Ok(service.HandleTextStoreUpdate(req));
}
