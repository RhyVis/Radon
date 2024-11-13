using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Response;
using Radon.Core.Processor.Interface;
using Radon.Core.Util;

namespace Radon.Arc.Controller;

[ApiController, Route("api/pdx")]
public class PdxController(IParadoxProcessor processor) : ControllerBase
{
    [HttpGet("test")]
    public UnsetRes GetTest()
    {
        var test = PdxLangParser.Create(["00_TESTER.LINE_EOF:0 \"Testing §rParser§! Item\""]);
        return new UnsetRes(test.Parse());
    }

    [HttpPost("parse/lang")]
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
}
