using NLog;
using OpenCCNET;

namespace Radon.Common.Utils;

public static class InitOpenCC
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public static void Setup()
    {
        _logger.Debug("Init OpenCC......");

        ZhConverter.Initialize();
    }
}