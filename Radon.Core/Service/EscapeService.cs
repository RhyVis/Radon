using Radon.Common.Core.DI;
using Radon.Common.Core.Extension;
using Radon.Core.Enums;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Processor.Interface;
using Radon.Core.Service.Interface;

namespace Radon.Core.Service;

[ServiceTransient]
public class EscapeService(IDictProcessor dictProcessor) : IEscapeService
{
    public EscapeRes HandleEscape(EscapeReq req)
    {
        var type = req.Data.Type.TryParseEnum(DictType.NONE);
        return (type == DictType.NONE)
            ? new EscapeRes(-1, "Invalid dict type", req.Data.Text)
            : new EscapeRes(dictProcessor.ProcessDict(type, req.Data.Text, req.Data.Encode));
    }
}
