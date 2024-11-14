using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FreeRedis;
using Masuit.Tools;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Radon.Common.Core.Config;
using Radon.Common.Core.DI;
using Radon.Security.Data.Entity;
using Radon.Security.Exceptions;
using Radon.Security.Model;
using Radon.Security.Service.Interface;
using RoleType = Radon.Security.Enums.RoleType;

namespace Radon.Security.Service;

[ServiceSingleton]
public class JwtService(IRedisClient cli) : IJwtService
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public Passport GenerateToken(User user)
    {
        var p = GenPassport(user);
        cli.Set(p.Token, p.UserId, AppSettings.Get().Security.Jwt.ExpireMinutes * 60);
        return p;
    }

    public long CheckToken(string token, bool checkSession = true)
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
            var userId =
                principal.FindFirst("userId")?.Value.ToInt64(long.MaxValue)
                ?? throw new CredentialRejectionException("User ID not found in token");
            if (userId == long.MaxValue)
            {
                throw new CredentialRejectionException("Invalid user ID in token");
            }

            return userId;
        }
        catch (SessionNotExistException)
        {
            _logger.Warn("Invalid token rejected by session check");
            throw;
        }
        catch (Exception ex)
        {
            _logger.Warn("Invalid token rejected by validation");
            throw new CredentialRejectionException("Invalid token", ex);
        }
    }

    public void InvalidateToken(string token) => cli.Del(token);

    private static Passport GenPassport(User user)
    {
        var conf = AppSettings.Get().Security.Jwt;
        var expr = DateTime.Now.AddMinutes(conf.ExpireMinutes);

        var claims = new List<Claim>
        {
            new("id", user.Id.ToString()),
            new("userId", user.Id.ToString()),
            new("role", user.Role.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: conf.Issuer,
            audience: conf.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: expr,
            signingCredentials: conf.SigningCredentials
        );
        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

        return new Passport(tokenStr, user.Id, expr);
    }
}
