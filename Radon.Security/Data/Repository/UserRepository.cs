using Radon.Data.Repository;
using Radon.Security.Data.Entity;
using Radon.Security.Exceptions;

namespace Radon.Security.Data.Repository;

public class UserRepository(IFreeSql fsql) : BaseIdRepository<User, long>(fsql)
{
    public User FindByUsername(string username)
    {
        return Select.Where(x => x.Username == username).First()
            ?? throw new UserNotFoundException(username);
    }
}
