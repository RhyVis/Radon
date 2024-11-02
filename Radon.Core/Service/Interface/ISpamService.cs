using Radon.Core.Model.Request;
using Radon.Core.Model.Response;

namespace Radon.Core.Service.Interface;

public interface ISpamService
{
    public SpamRes HandleSpam(SpamReq req);
}
