using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FreeRedis;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Radon.Common.Core.Config;
using Radon.Common.Core.DI;
using Radon.Security.Exceptions;
using Radon.Security.Model;
using Radon.Security.Service.Interface;
using RoleType = Radon.Security.Enums.RoleType;

namespace Radon.Security.Service;

[ServiceSingleton]
public class JwtService(IRedisClient cli) : IJwtService
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public Passport GenerateAnonymousToken()
    {
        var p = GenPassport([new Claim("role", RoleType.Anonymous.ToString())]);
        cli.Set(p.Token, p.Role, AppSettings.Get().Security.Jwt.ExpireMinutes * 60);
        return p;
    }

    public bool CheckToken(string token, bool checkSession = false)
    {
        var conf = AppSettings.Get().Security.Jwt;
        var handler = new JwtSecurityTokenHandler();
        try
        {
            handler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = conf.Issuer,
                    ValidAudience = conf.Audience,
                    IssuerSigningKey = conf.SigningCredentials.Key,
                },
                out _
            );
            if (checkSession && cli.Get(token) == null)
            {
                throw new SessionNotExistException();
            }
        }
        catch (SessionNotExistException)
        {
            _logger.Warn("Invalid token rejected by session check");
            return false;
        }
        catch (Exception)
        {
            _logger.Warn("Invalid token rejected by validation");
            return false;
        }
        return true;
    }

    private static Passport GenPassport(List<Claim> claims)
    {
        var conf = AppSettings.Get().Security.Jwt;
        var expr = DateTime.Now.AddMinutes(conf.ExpireMinutes);

        var token = new JwtSecurityToken(
            issuer: conf.Issuer,
            audience: conf.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: expr,
            signingCredentials: conf.SigningCredentials
        );

        return new Passport(new JwtSecurityTokenHandler().WriteToken(token), expr);
    }
}
