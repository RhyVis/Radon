using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Arc.Controller.Api;

[Authorize]
[ApiController]
[Route("api/text-store")]
public class SaveController(ITextStoreService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<TextStoreRes>(200)]
    public IActionResult Query(TextStoreReq req) => Ok(service.HandleTextStoreQuery(req));

    [HttpPut]
    [ProducesResponseType<StateRes>(200)]
    public IActionResult Update(TextStoreReq req) => Ok(service.HandleTextStoreUpdate(req));
}
