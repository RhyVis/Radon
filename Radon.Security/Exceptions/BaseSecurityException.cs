using Radon.Common.Core.Exception;

namespace Radon.Security.Exceptions;

public abstract class BaseSecurityException : BaseException
{
    protected BaseSecurityException(string msg)
        : base(msg) { }

    protected BaseSecurityException(string msg, Exception inner)
        : base(msg, inner) { }
}
