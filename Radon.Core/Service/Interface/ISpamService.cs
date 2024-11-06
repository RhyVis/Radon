using Radon.Core.Model.Request;
using Radon.Core.Model.Response;

namespace Radon.Core.Service.Interface;

public interface ISpamService
{
    public SpamRes Fetch(SpamFetchReq req);

    public bool Append(SpamAppendReq req);
}
