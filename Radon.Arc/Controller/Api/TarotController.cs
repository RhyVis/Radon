﻿using Microsoft.AspNetCore.Mvc;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Arc.Controller.Api;

[ApiController]
[Route("api/tarot")]
public class TarotController(ITarotService tarotService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<TarotRes>(200)]
    public IActionResult Get()
    {
        return Ok(tarotService.HandleTarot());
    }

    [HttpPost]
    [ProducesResponseType<TarotRes>(200)]
    public IActionResult Post(TarotReq req)
    {
        return Ok(tarotService.HandleTarotComplex(req));
    }

    [HttpGet("info")]
    [ProducesResponseType<UnsetRes>(200)]
    public IActionResult GetInfo()
    {
        return Ok(tarotService.HandleTarotInfo());
    }
}