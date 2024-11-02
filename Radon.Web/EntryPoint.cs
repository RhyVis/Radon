using Radon.Reckon.Economy;
using Radon.Web.Setup;

namespace Radon.Web;

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
