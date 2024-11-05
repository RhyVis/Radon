using Radon.Security.Model;

namespace Radon.Security.Service.Interface;

public interface IUsernameAuthService
{
    /// <summary>
    /// Register a new user into database
    /// </summary>
    void Register(string username, string password);

    /// <summary>
    /// Check the username and password and return a Passport with JWT
    /// </summary>
    /// <returns>Passport with JWT</returns>
    Passport Authenticate(string username, string password);

    /// <summary>
    /// Check the token and return a new Passport with JWT, with old one removing in session
    /// </summary>
    /// <returns>New Passport with JWT</returns>
    Passport Refresh(string token);
}
