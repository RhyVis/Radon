using Masuit.Tools.Strings;

namespace Radon.Common.Utils;

public static class RandomUtil
{
    public static string RandomString()
    {
        return new NumberFormater(62).ToString(new Random().Next(100000, int.MaxValue));
    }
}