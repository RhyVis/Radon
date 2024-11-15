using NLog;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Radon.Common.Core.DI;
using Radon.Security.Data.Entity;
using Radon.Security.Data.Repository;
using Radon.Security.Model;
using Radon.Security.Service.Interface;

namespace Radon.Security.Service;

[ServiceScoped]
public class UsernameAuthService(UserRepository repo, IJwtService jwt) : IUsernameAuthService
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public void Register(string username, string password)
    {
        try
        {
            var user = new User { Username = username, Password = GenHash(password) };
            repo.Insert(user);
            _logger.Info($"New user {username} registered");
        }
        catch (Exception e)
        {
            _logger.Error(e, $"Failed to register user {username}");
            throw;
        }
    }

    public void Unregister(string username)
    {
        try
        {
            var user = repo.FindByUsername(username);
            repo.Delete(user);

            try
            {
                jwt.InvalidateAllToken(user.Id);
            }
            catch (Exception) // Just skip
            {
                _logger.Warn("Failed to invalidate token for user {username}");
            }

            _logger.Info($"User {username} unregistered");
        }
        catch (Exception e)
        {
            _logger.Error(e, $"Failed to unregister user {username}");
            throw;
        }
    }

    public void ChangePassword(string username, string newPassword)
    {
        try
        {
            var user = repo.FindByUsername(username);
            user.Password = GenHash(newPassword);
            repo.Update(user);
            _logger.Info($"Password changed for user {username}");
        }
        catch (Exception e)
        {
            _logger.Error(e, $"Failed to change password for user {username}");
            throw;
        }
    }

    public void LogoutById(long userId) => jwt.InvalidateAllToken(userId);

    public void LogoutByName(string username) => LogoutById(repo.FindByUsername(username).Id);

    public void LogoutByToken(string token) => LogoutById(jwt.CheckToken(token, false, false));

    public Passport Authenticate(string username, string password)
    {
        try
        {
            var user = repo.FindByUsername(username);
            CheckHash(user.Password, password);

            return jwt.GenerateToken(user);
        }
        catch (Exceptions.CredentialRejectionException)
        {
            _logger.Warn($"Password does not match for user {username}");
            throw;
        }
    }

    public Passport Refresh(string token)
    {
        var userId = jwt.CheckToken(token);

        return Refresh(token, userId);
    }

    public Passport Refresh(string token, long userId)
    {
        var user = repo.FindByUserId(userId);

        jwt.InvalidateToken(token);

        return jwt.GenerateToken(user);
    }

    private static string GenHash(string password, int cost = 11)
    {
        var salt = new byte[16];
        new SecureRandom().NextBytes(salt);
        var hash = OpenBsdBCrypt.Generate(password.ToCharArray(), salt, cost);
        return hash;
    }

    private static void CheckHash(string hash, string password)
    {
        if (!OpenBsdBCrypt.CheckPassword(hash, password.ToCharArray()))
        {
            throw new Exceptions.CredentialRejectionException("Password does not match");
        }
    }
}
