﻿using Microsoft.AspNetCore.Mvc;
using Radon.Common.Core.Config;

namespace Radon.Arc.Controller;

[ApiController]
[Route("api")]
public class ApiRootController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<object>(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok(new { AppSettings.Get().Name, AppSettings.Get().Version });
    }
}