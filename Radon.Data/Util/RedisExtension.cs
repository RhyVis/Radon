using FreeRedis;

namespace Radon.Data.Util;

public static class RedisExtension
{
    public static List<string> ScamByVal(this IRedisClient cli, long target, int count = 10)
    {
        var match = new List<string>();
        var cursor = 0L;
        do
        {
            var scan = cli.Scan(cursor, "*", count, null);
            cursor = scan.cursor;
            match.AddRange(scan.items.Where(key => cli.Get<long>(key) == target));
        } while (cursor != 0);

        return match;
    }

    public static List<string> ScamByVal(this IRedisClient cli, string target, int count = 10)
    {
        var match = new List<string>();
        var cursor = 0L;
        do
        {
            var scan = cli.Scan(cursor, "*", count, null);
            cursor = scan.cursor;
            match.AddRange(scan.items.Where(key => cli.Get<string>(key) == target));
        } while (cursor != 0);

        return match;
    }
}
