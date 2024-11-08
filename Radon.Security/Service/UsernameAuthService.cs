﻿using NLog;
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
        var user = new User { Username = username, Password = GenHash(password) };
        try
        {
            repo.Insert(user);
            _logger.Info($"New user {username} registered");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Failed to register user {username}");
            throw;
        }
    }

    public Passport Authenticate(string username, string password)
    {
        var user = repo.FindByUsername(username);

        try
        {
            CheckHash(user.Password, password);
        }
        catch (Exceptions.InvalidCredentialException)
        {
            _logger.Warn($"Password does not match for user {username}");
            throw;
        }

        return jwt.GenerateToken(user);
    }

    public Passport Refresh(string token)
    {
        var username = jwt.CheckToken(token, true);
        var user = repo.FindByUsername(username);
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
            throw new Exceptions.InvalidCredentialException("Password does not match");
        }
    }
}
