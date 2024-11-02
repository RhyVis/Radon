using Radon.Core.Enums;

namespace Radon.Core.Processor.Interface;

public interface IDictProcessor
{
    string ProcessDict(DictType type, string text, bool encode);
}
