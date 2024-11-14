using Radon.Data.Repository;
using Radon.Security.Data.Entity;
using Radon.Security.Exceptions;

namespace Radon.Security.Data.Repository;

public class UserRepository(IFreeSql fsql) : BaseIdRepository<User, long>(fsql)
{
    public User FindByUsername(string username) =>
        Select.Where(x => x.Username == username).First()
        ?? throw new UserNotFoundException(username);

    public User FindByUserId(long userId) =>
        Select.Where(x => x.Id == userId).First()
        ?? throw new UserNotFoundException($"Id: {userId}");
}
