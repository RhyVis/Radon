using Radon.Common.Core.Exception;

namespace Radon.Core.Data.Repository.Exception;

public class DataInitException(string message) : BaseException(message)
{
}