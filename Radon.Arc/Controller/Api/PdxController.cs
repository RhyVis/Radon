using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Radon.Arc.Util;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Processor.Interface;
using Radon.Core.Service.Interface;
using Radon.Core.Util;

namespace Radon.Arc.Controller.Api;

[ApiController]
[Route("api/pdx")]
public class PdxController(IParadoxProcessor processor, IPdxService service, IMemoryCache cache) : ControllerBase
{
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(PlainNumberRes.Of(0));
    }

    [HttpPost("parse/lang")]
    [ProducesResponseType<UnsetRes>(StatusCodes.Status200OK)]
    public IActionResult ParseLang([FromForm] IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            var dummy = new List<PdxLangParsedItem>
            {
                new() { Namespace = ["empty"], Value = "empty" }
            };
            return Ok(new UnsetRes(dummy));
        }

        using var stream = file.OpenReadStream();
        var ls = processor.ParseLang(stream);

        return Ok(new UnsetRes(ls));
    }

    [HttpPost("parse/event")]
    [ProducesResponseType<UnsetRes>(StatusCodes.Status200OK)]
    public IActionResult ParseEvent([FromForm] IFormFile? file)
    {
        if (file is null || file.Length is 0)
        {
            var dummy = new List<PdxLangEventItem>
            {
                new()
                {
                    Key = "dummy",
                    Name = "dummy",
                    Desc = "dummy",
                    Options = []
                }
            };
            return Ok(new UnsetRes(dummy));
        }

        using var stream = file.OpenReadStream();
        var ls = processor.ParseEvent(stream);

        return Ok(new UnsetRes(ls));
    }

    [Authorize]
    [HttpGet("parse/lang/{id:long}")]
    [ProducesResponseType<UnsetRes>(StatusCodes.Status200OK)]
    public IActionResult ParseLangFromTextStorage(long id)
    {
        var cacheKey = $"pdx_lang_item_{id}";
        var r = cache.GetOrCreate(cacheKey, entry =>
        {
            var parsed = processor.ParseLangFromTextStorage(id);
            entry.SlidingExpiration = TimeSpan.FromHours(2);
            entry.Value = parsed;
            return parsed;
        });

        return Ok(new UnsetRes(r!));
    }

    [Authorize]
    [HttpGet("parse/event/{id:long}")]
    [ProducesResponseType<UnsetRes>(StatusCodes.Status200OK)]
    public IActionResult ParseEventFromTextStorage(long id)
    {
        var cacheKey = $"pdx_event_item_{id}";
        var r = cache.GetOrCreate(cacheKey, entry =>
        {
            var events = processor.ParseEventFromTextStorage(id);
            entry.SlidingExpiration = TimeSpan.FromHours(2);
            entry.Value = events;
            return events;
        });

        return Ok(new UnsetRes(r!));
    }

    [Authorize]
    [HttpPut("parse/lang/replacer")]
    [ProducesResponseType<StateRes>(StatusCodes.Status200OK)]
    public IActionResult SetLangReplacer(PlainTextReq req)
    {
        var userId = HttpContext.GetAuthenticatedUserId();
        service.SetLangParserReplacer(userId, req.Data);

        return Ok(StateRes.OfSuccess());
    }

    [Authorize]
    [HttpGet("parse/lang/replacer")]
    [ProducesResponseType<PlainTextRes>(StatusCodes.Status200OK)]
    public IActionResult GetLangReplacer()
    {
        var userId = HttpContext.GetAuthenticatedUserId();
        var replacerJson = service.GetLangParserReplacer(userId);

        return Ok(PlainTextRes.Of(replacerJson));
    }
}
