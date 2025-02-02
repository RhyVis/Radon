﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FreeRedis;
using Masuit.Tools;
using Microsoft.IdentityModel.Tokens;
using Radon.Common.Core.Config;
using Radon.Common.Core.DI;
using Radon.Data.Util;
using Radon.Security.Data.Entity;
using Radon.Security.Exceptions;
using Radon.Security.Model;
using Radon.Security.Service.Interface;

namespace Radon.Security.Service;

[ServiceSingleton]
public class JwtService(IRedisClient cli) : IJwtService
{
    public Passport GenerateToken(User user)
    {
        var p = GenPassport(user);
        cli.Set(p.Token, p.UserId, AppSettings.Get().Security.Jwt.ExpireMinutes * 60);
        return p;
    }

    public long CheckToken(string token, bool checkSession = true, bool checkLifetime = true)
    {
        var conf = AppSettings.Get().Security.Jwt;
        var handler = new JwtSecurityTokenHandler();
        try
        {
            var principal = handler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = checkLifetime,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = conf.Issuer,
                    ValidAudience = conf.Audience,
                    IssuerSigningKey = conf.SigningCredentials.Key
                },
                out _
            );
            if (checkSession && cli.Get(token) == null) throw new SessionNotExistException();
            var userId =
                principal.FindFirst("userId")?.Value.ToInt64(long.MaxValue)
                ?? throw new CredentialRejectionException("User ID not found in token");
            CredentialRejectionException.CheckId(userId);

            return userId;
        }
        catch (SessionNotExistException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new CredentialRejectionException("Invalid token", ex);
        }
    }

    public void InvalidateToken(string token)
    {
        cli.Del(token);
    }

    public void InvalidateAllToken(long userId)
    {
        if (cli.ScamByVal(userId, out var find)) find.ForEach(key => cli.Del(key));
    }

    private static Passport GenPassport(User user)
    {
        var conf = AppSettings.Get().Security.Jwt;
        var expr = DateTime.Now.AddMinutes(conf.ExpireMinutes);

        var claims = new List<Claim>
        {
            new("id", user.Id.ToString()),
            new("userId", user.Id.ToString()),
            new("role", user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            conf.Issuer,
            conf.Audience,
            claims,
            DateTime.Now,
            expr,
            conf.SigningCredentials
        );
        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token) ??
                       throw new CredentialRejectionException("Token generation failed");

        return new Passport(tokenStr, user.Id, user.Extra.MapToPassportExtra());
    }
}
