using Masuit.Tools;
using Radon.Security.Data.Entity;
using Radon.Security.Data.Repository;
using Radon.Security.Exceptions;

namespace Radon.Arc.Util;

public static class AuthExtension
{
    /// <summary>
    /// Get authenticated user ID from claims.
    /// </summary>
    /// <returns>The user ID, can be used to fetch user.</returns>
    /// <exception cref="CredentialRejectionException">Cannot find user</exception>
    public static long GetAuthenticatedUserId(this HttpContext context)
    {
        var claims = context.User.Claims;
        var userId =
            claims.FirstOrDefault(c => c.Type == "userId")?.Value.ToInt64(long.MaxValue)
            ?? throw new CredentialRejectionException("No ID found in claims.");
        CredentialRejectionException.CheckId(userId);

        return userId;
    }

    /// <summary>
    /// Get authenticated user from claims.
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="repo">The UserRepository, from DI</param>
    /// <returns>User Entity</returns>
    public static User GetAuthenticatedUser(this HttpContext context, UserRepository repo)
    {
        var userId = context.GetAuthenticatedUserId();
        return repo.FindByUserId(userId);
    }

    /// <summary>
    /// Get authenticated token from headers.
    /// </summary>
    /// <exception cref="CredentialRejectionException">No token found</exception>
    public static string GetAuthenticatedToken(this HttpContext context)
    {
        var header = context.Request.Headers.Authorization.FirstOrDefault();
        if (header.IsNullOrEmpty() || !header!.StartsWith("Bearer "))
        {
            throw new CredentialRejectionException("No token found in headers.");
        }
        return header["Bearer ".Length..];
    }
}
