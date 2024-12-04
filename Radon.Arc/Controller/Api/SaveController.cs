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
    [ProducesResponseType<TextStoreRes>(StatusCodes.Status200OK)]
    public IActionResult Query(TextStoreReq req)
    {
        return Ok(service.HandleTextStoreQuery(req));
    }

    [HttpPut]
    [ProducesResponseType<StateRes>(StatusCodes.Status200OK)]
    public IActionResult Update(TextStoreReq req)
    {
        return Ok(service.HandleTextStoreUpdate(req));
    }

    [HttpGet]
    [ProducesResponseType<UnsetRes>(StatusCodes.Status200OK)]
    public IActionResult All()
    {
        return Ok(service.GetAll());
    }
}
