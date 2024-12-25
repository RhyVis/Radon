using Masuit.Tools;
using NLog;
using Radon.Common.Core.DI;
using Radon.Security.Data.Entity;
using Radon.Security.Data.Repository;
using Radon.Security.Exceptions;
using Radon.Security.Service.Interface;

namespace Radon.Security.Service;

[ServiceScoped]
public class UserService(UserRepository repo) : IUserService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public void AppendImageToken(long userId, string token)
    {
        var user = repo.FindByUserId(userId) ?? throw new UserNotFoundException(userId.ToString());
        var extraObj = user.Extra?.DeepClone(true) ?? new UserExtra();

        extraObj.ImageToken = token;
        user.Extra = extraObj;

        repo.Update(user);
        Logger.Info($"User {userId} appended image token {token}");
    }
}
