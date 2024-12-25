using Radon.Core.Model.Response;
using Radon.Security.Model;

namespace Radon.Core.Util;

public static class PassportExtension
{
    public static UnsetRes OfRes(this Passport passport)
    {
        return new UnsetRes(passport);
    }
}
