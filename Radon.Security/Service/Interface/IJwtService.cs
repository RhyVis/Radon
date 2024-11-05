using Radon.Security.Data.Entity;
using Radon.Security.Model;

namespace Radon.Security.Service.Interface;

public interface IJwtService
{
    /// <summary>
    /// Generate a JWT token for the user
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>Passport containing the jwt</returns>
    Passport GenerateToken(User user);

    /// <summary>
    /// Check the token and return the username
    /// </summary>
    /// <param name="token">JWT</param>
    /// <param name="checkSession">Check if the session exists</param>
    /// <returns>Username</returns>
    string CheckToken(string token, bool checkSession = true);

    /// <summary>
    /// Invalidate the token
    /// </summary>
    /// <param name="token">JWT</param>
    void InvalidateToken(string token);
}
