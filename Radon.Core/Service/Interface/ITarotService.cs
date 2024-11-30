using Radon.Core.Model.Request;
using Radon.Core.Model.Response;

namespace Radon.Core.Service.Interface;

public interface ITarotService
{
    TarotRes HandleTarot();

    TarotRes HandleTarotComplex(TarotReq req);

    UnsetRes HandleTarotInfo();

    UnsetRes GetTarotDeckData(string name, bool full, bool shuffle);
}