using Radon.Security.Model;

namespace Radon.Security.Service.Interface;

public interface IUsernameAuthService
{
    void Register(string username, string password);
    Passport Authenticate(string username, string password);
}
