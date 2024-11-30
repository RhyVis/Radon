using NLog;
using NLog.Web;
using Radon.Arc;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

logger.Info("Init Radon......");

// Resharper disable once StringLiteralTypo
Console.WriteLine(
    """

    ooooooooo.                   .o8                        
    `888   `Y88.                "888                        
     888   .d88'  .oooo.    .oooo888   .ooooo.  ooo. .oo.   
     888ooo88P'  `P  )88b  d88' `888  d88' `88b `888P"Y88b  
     888`88b.     .oP"888  888   888  888   888  888   888  
     888  `88b.  d8(  888  888   888  888   888  888   888  
    o888o  o888o `Y888""8o `Y8bod88P" `Y8bod8P' o888o o888o 

    """
);

// Resharper restore StringLiteralTypo

EntryPoint.Exec(args);