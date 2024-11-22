using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radon.Arc.Util;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Processor.Interface;
using Radon.Core.Service.Interface;
using Radon.Core.Util;

namespace Radon.Arc.Controller;

[ApiController, Route("api/pdx")]
public class PdxController(IParadoxProcessor processor, IPdxService service) : ControllerBase
{
    [HttpGet("test")]
    [ProducesResponseType(typeof(UnsetRes), StatusCodes.Status200OK)]
    public IActionResult GetTest()
    {
        var test = PdxLangParser.Create(["00_TESTER.LINE_EOF_O:0 \"Testing §rParser§! Item\""]);
        return Ok(new UnsetRes(test.Parse()));
    }

    [HttpPost("parse/lang")]
    [ProducesResponseType(typeof(UnsetRes), StatusCodes.Status200OK)]
    public IActionResult ParseLang([FromForm] IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            var dummy = new List<PdxLangParsedItem>
            {
                new() { Namespace = ["empty"], Value = "empty" },
            };
            return Ok(new UnsetRes(dummy));
        }

        using var stream = file.OpenReadStream();
        var ls = processor.ParseLang(stream);

        return Ok(new UnsetRes(ls));
    }

    [Authorize]
    [HttpPut("parse/lang/replacer")]
    [ProducesResponseType(typeof(StateRes), StatusCodes.Status200OK)]
    public IActionResult SetLangReplacer(PlainTextReq req)
    {
        var userId = HttpContext.GetAuthenticatedUserId();
        service.SetLangParserReplacer(userId, req.Data);

        return Ok(new StateRes());
    }

    [Authorize]
    [HttpGet("parse/lang/replacer")]
    [ProducesResponseType(typeof(PlainTextRes), StatusCodes.Status200OK)]
    public IActionResult GetLangReplacer()
    {
        var userId = HttpContext.GetAuthenticatedUserId();
        var replacerJson = service.GetLangParserReplacer(userId);

        return Ok(PlainTextRes.Of(replacerJson));
    }
}
