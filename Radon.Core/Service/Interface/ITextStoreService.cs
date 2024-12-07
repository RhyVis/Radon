using Radon.Core.Model.Request;
using Radon.Core.Model.Response;

namespace Radon.Core.Service.Interface;

public interface ITextStoreService
{
    TextStoreRes Query(long id);

    UnsetRes QueryAll();

    StateRes Update(TextStoreReq req);

    StateRes Delete(long id);
}
