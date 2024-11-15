using Radon.Security.Model;

namespace Radon.Security.Service.Interface;

public interface IUsernameAuthService
{
    /// <summary>
    /// Register a new user into database
    /// </summary>
    void Register(string username, string password);

    /// <summary>
    /// Unregister a user from database
    /// </summary>
    void Unregister(string username);

    /// <summary>
    /// Change the password of a user
    /// </summary>
    void ChangePassword(string username, string newPassword);

    /// <summary>
    /// Clear token of a user
    /// </summary>
    void Logout(string username);

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
