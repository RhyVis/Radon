using FreeRedis;
using Masuit.Tools;

namespace Radon.Data.Util;

public static class RedisExtension
{
    /// <summary>
    /// Find all matching keys by value
    /// </summary>
    public static bool ScamByVal(
        this IRedisClient cli,
        long target,
        out List<string> find,
        int count = 10
    )
    {
        var match = new List<string>();
        var cursor = 0L;
        do
        {
            var scan = cli.Scan(cursor, "*", count, null);
            cursor = scan.cursor;
            match.AddRange(scan.items.Where(key => cli.Get<long>(key) == target));
        } while (cursor != 0);

        find = match;

        return find.IsNullOrEmpty();
    }

    /// <summary>
    /// Find all matching keys by value
    /// </summary>
    public static bool ScamByVal(
        this IRedisClient cli,
        string target,
        out List<string> find,
        int count = 10
    )
    {
        var match = new List<string>();
        var cursor = 0L;
        do
        {
            var scan = cli.Scan(cursor, "*", count, null);
            cursor = scan.cursor;
            match.AddRange(scan.items.Where(key => cli.Get<string>(key) == target));
        } while (cursor != 0);

        find = match;

        return find.IsNullOrEmpty();
    }
}
