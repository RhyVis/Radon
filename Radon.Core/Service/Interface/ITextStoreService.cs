using Radon.Core.Model.Request;
using Radon.Core.Model.Response;

namespace Radon.Core.Service.Interface;

public interface ITextStoreService
{
    TextStoreRes HandleTextStoreQuery(TextStoreReq req);

    StateRes HandleTextStoreUpdate(TextStoreReq req);

    UnsetRes GetAll();
}
