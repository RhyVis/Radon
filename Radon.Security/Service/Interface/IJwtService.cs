using Radon.Security.Model;

namespace Radon.Security.Service.Interface;

public interface IJwtService
{
    Passport GenerateAnonymousToken();
}
