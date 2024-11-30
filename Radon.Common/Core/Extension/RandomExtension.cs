namespace Radon.Common.Core.Extension;

public static class RandomExtension
{
    public static bool NextBool(this Random rand)
    {
        return rand.NextDouble() >= 0.5;
    }
}