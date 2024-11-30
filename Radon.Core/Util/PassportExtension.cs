using Radon.Core.Model.Response;
using Radon.Security.Model;

namespace Radon.Core.Util;

public static class PassportExtension
{
    public static PlainTextRes OfRes(this Passport passport)
    {
        return PlainTextRes.Of(passport.Token);
    }
}