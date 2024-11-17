using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radon.Arc.Util;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Security.Service.Interface;

namespace Radon.Arc.Controller;

[Authorize]
[ApiController]
[Route("api/auth")]
public class AuthenticationController(IUsernameAuthService service) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(PlainTextRes), StatusCodes.Status200OK)]
    public IActionResult Login(UsernameReq req)
    {
        var p = service.Authenticate(req.Data.Username, req.Data.Password);
        return Ok(new PlainTextRes(p.Token));
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
    [ProducesResponseType(typeof(PlainTextRes), StatusCodes.Status200OK)]
    public IActionResult Refresh()
    {
        var token = HttpContext.GetAuthenticatedToken();
        var userID = HttpContext.GetAuthenticatedUserId();
        var p = service.Refresh(token, userID);
        return Ok(new PlainTextRes(p.Token));
    }
}
