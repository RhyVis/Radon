using Radon.Core.Model.Request;
using Radon.Core.Model.Response;

namespace Radon.Core.Service.Interface;

public interface ISimpleAuthenticationService
{
    StateRes HandleSimpleAuthentication(PlainTextReq req);
}
