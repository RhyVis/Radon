using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Security.Service.Interface;

namespace Radon.Arc.Controller;

[ApiController]
[Route("api/auth")]
public class AuthenticationController(IUsernameAuthService service, IJwtService jwt)
    : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(PlainTextRes), StatusCodes.Status200OK)]
    public IActionResult Login(UsernameReq req)
    {
        var p = service.Authenticate(req.Data.Username, req.Data.Password);
        return Ok(new PlainTextRes(p.Token));
    }

    [Authorize]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Register(UsernameReq req)
    {
        service.Register(req.Data.Username, req.Data.Password);
        return NoContent();
    }

    [HttpPost("validate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Validate(PlainTextReq req)
    {
        jwt.CheckToken(req.Data);
        return NoContent();
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(PlainTextRes), StatusCodes.Status200OK)]
    public IActionResult Refresh(PlainTextReq req)
    {
        var p = service.Refresh(req.Data);
        return Ok(new PlainTextRes(p.Token));
    }
}
