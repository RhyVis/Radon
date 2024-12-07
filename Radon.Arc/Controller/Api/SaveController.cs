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
    [HttpGet("{id:long}")]
    [ProducesResponseType<TextStoreRes>(StatusCodes.Status200OK)]
    public IActionResult Query(long id)
    {
        return Ok(service.Query(id));
    }

    [HttpGet]
    [ProducesResponseType<UnsetRes>(StatusCodes.Status200OK)]
    public IActionResult QueryAll()
    {
        return Ok(service.QueryAll());
    }

    [HttpPut]
    [ProducesResponseType<StateRes>(StatusCodes.Status200OK)]
    public IActionResult Update(TextStoreReq req)
    {
        return Ok(service.Update(req));
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType<StateRes>(StatusCodes.Status200OK)]
    public IActionResult Delete(long id)
    {
        return Ok(service.Delete(id));
    }
}
