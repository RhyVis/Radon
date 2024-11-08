using Radon.Arc.Setup;

namespace Radon.Arc;

public static class EntryPoint
{
    public static void Setup(string[] args)
    {
        WebApplication
            .CreateBuilder(args)
            .SetupBuilder()
            .SetupService()
            .Build()
            .SetupApplication()
            .Run();
    }
}
