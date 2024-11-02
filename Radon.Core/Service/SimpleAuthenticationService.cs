using System.Security.Cryptography;
using System.Text;
using Masuit.Tools;
using NLog;
using Radon.Common.Core.Config;
using Radon.Common.Core.DI;
using Radon.Common.Utils;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Core.Service;

[ServiceSingleton]
public class SimpleAuthenticationService : ISimpleAuthenticationService
{
    private static Logger _logger = LogManager.GetCurrentClassLogger();

    public SimpleAuthenticationService()
    {
        var pw = AppSettings.Get().Security.AuthToken;

        if (pw.IsNullOrEmpty())
        {
            pw = RandomUtil.RandomString();
            _logger.Warn($"No password set for SimpleAuth, generating as {pw}");
            _logger.Info("Note that this password will be lost on restart");
        }

        _hashPassword = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(pw))).ToLower();
    }

    private string _hashPassword;

    public StateRes HandleSimpleAuthentication(PlainTextReq req)
    {
        var ca = _hashPassword == req.Data;
        _logger.Info($"SimpleAuth: {(ca ? "Success" : "Failed")}");
        return new StateRes(ca);
    }
}
