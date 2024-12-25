namespace Radon.Security.Service.Interface;

public interface IUserService
{
    void AppendImageToken(long userId, string token);
}