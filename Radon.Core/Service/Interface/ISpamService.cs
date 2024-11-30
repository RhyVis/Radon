using Radon.Core.Model.Request;
using Radon.Core.Model.Response;

namespace Radon.Core.Service.Interface;

public interface ISpamService
{
    SpamRes Fetch(SpamFetchReq req);
    SpamRes FetchPrecise(SpamFetchPreciseReq req);
    long Append(SpamAppendReq req);
}