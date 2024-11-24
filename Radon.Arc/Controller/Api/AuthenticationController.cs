using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radon.Arc.Util;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Util;
using Radon.Security.Service.Interface;

namespace Radon.Arc.Controller.Api;

[Authorize]
[ApiController, Route("api/auth")]
public class AuthenticationController(IUsernameAuthService service) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType<PlainTextRes>(StatusCodes.Status200OK)]
    public IActionResult Login(UsernameReq req)
    {
        var p = service.Authenticate(req.Data.Username, req.Data.Password);
        return Ok(p.OfRes());
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Logout()
    {
        var userId = HttpContext.GetAuthenticatedUserId();
        service.LogoutById(userId);

        return NoContent();
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Register(UsernameReq req)
    {
        service.Register(req.Data.Username, req.Data.Password);
        return NoContent();
    }

    [HttpPost("validate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Validate() => NoContent();

    [HttpPost("refresh")]
    [ProducesResponseType<PlainTextRes>(StatusCodes.Status200OK)]
    public IActionResult Refresh()
    {
        var token = HttpContext.GetAuthenticatedToken();
        var userId = HttpContext.GetAuthenticatedUserId();
        var p = service.Refresh(token, userId);
        return Ok(p.OfRes());
    }
}
