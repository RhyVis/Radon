using Masuit.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Processor.Interface;
using Radon.Core.Service.Interface;
using Radon.Core.Util;
using Radon.Security.Exceptions;

namespace Radon.Arc.Controller;

[ApiController, Route("api/pdx")]
public class PdxController(IParadoxProcessor processor, IPdxService service) : ControllerBase
{
    [HttpGet("test")]
    [ProducesResponseType(typeof(UnsetRes), StatusCodes.Status200OK)]
    public UnsetRes GetTest()
    {
        var test = PdxLangParser.Create(["00_TESTER.LINE_EOF:0 \"Testing §rParser§! Item\""]);
        return new UnsetRes(test.Parse());
    }

    [HttpPost("parse/lang")]
    [ProducesResponseType(typeof(UnsetRes), StatusCodes.Status200OK)]
    public UnsetRes ParseLang([FromForm] IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            var dummy = new List<PdxLangParsedItem>
            {
                new() { Namespace = ["empty"], Value = "empty" },
            };
            return new UnsetRes(dummy);
        }

        using var stream = file.OpenReadStream();
        var ls = processor.ParseLang(stream);

        return new UnsetRes(ls);
    }

    [Authorize]
    [HttpPut("parse/lang/replacer")]
    [ProducesResponseType(typeof(StateRes), StatusCodes.Status200OK)]
    public IActionResult SetLangReplacer(PlainTextReq req)
    {
        var claims = HttpContext.User.Claims;
        var userId =
            claims.FirstOrDefault(c => c.Type == "userId")?.Value.ToInt64(long.MaxValue)
            ?? throw new CredentialRejectionException("No ID found in claims.");
        CredentialRejectionException.CheckId(userId);

        service.SetLangParserReplacer(userId, req.Data);

        return Ok(new StateRes());
    }

    [Authorize]
    [HttpGet("parse/lang/replacer")]
    [ProducesResponseType(typeof(PlainTextRes), StatusCodes.Status200OK)]
    public IActionResult GetLangReplacer()
    {
        var claims = HttpContext.User.Claims;
        var userId =
            claims.FirstOrDefault(c => c.Type == "userId")?.Value.ToInt64(long.MaxValue)
            ?? throw new CredentialRejectionException("No ID found in claims.");
        CredentialRejectionException.CheckId(userId);

        var replacerJson = service.GetLangParserReplacer(userId);

        return Ok(new PlainTextRes(replacerJson));
    }
}
