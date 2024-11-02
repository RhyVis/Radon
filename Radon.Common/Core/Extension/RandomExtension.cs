namespace Radon.Common.Core.Extension;

public static class RandomExtension
{
    public static bool NextBool(this Random rand) => rand.NextDouble() >= 0.5;
}
