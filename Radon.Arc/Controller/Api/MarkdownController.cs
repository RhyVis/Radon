using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Arc.Controller.Api;

[ApiController, Route("api/md")]
public class MarkdownController(IMarkdownService md) : ControllerBase
{
    [HttpGet, Route("{path}")]
    [ProducesResponseType<MdRes>(StatusCodes.Status200OK)]
    public IActionResult Provide(string path) => Ok(new MdRes { Data = md.ProvideContent(path) });

    [HttpPut, Route("{path}"), Authorize]
    [ProducesResponseType<PlainTextRes>(StatusCodes.Status200OK)]
    public IActionResult Update(string path, MdReq req)
    {
        var d = req.Data;
        var p = md.UpdateContent(d.Path, d.Name, d.Desc, d.Content);
        return Ok(PlainTextRes.Of(p));
    }

    [HttpGet, Route("check/{path}")]
    [ProducesResponseType<StateRes>(StatusCodes.Status200OK)]
    public IActionResult Check(string path) => Ok(StateRes.Of(md.CheckContent(path)));

    [HttpGet, Route("index")]
    [ProducesResponseType<UnsetRes>(StatusCodes.Status200OK)]
    public IActionResult Index() => Ok(new UnsetRes(md.ListIndex()));
}
