using Radon.Common.Core.Exception;

namespace Radon.Core.Exceptions;

public class ServiceFailureException : BaseException
{
    public ServiceFailureException(string message) : base(message)
    {
    }

    public ServiceFailureException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
