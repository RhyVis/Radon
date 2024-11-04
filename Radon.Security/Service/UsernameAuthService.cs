using Radon.Common.Core.DI;
using Radon.Security.Data.Repository;
using Radon.Security.Model;
using Radon.Security.Service.Interface;

namespace Radon.Security.Service;

[ServiceScoped]
public class UsernameAuthService(UserRepository repo) : IUsernameAuthService
{
    public void Register(string username, string password)
    {
        throw new NotImplementedException();
    }

    public Passport Authenticate(string username, string password)
    {
        var user = repo.FindByUsername(username);
        throw new NotImplementedException();
    }
}
