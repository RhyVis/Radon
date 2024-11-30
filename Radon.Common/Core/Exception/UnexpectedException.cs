namespace Radon.Common.Core.Exception;

public class UnexpectedException(string msg) : BaseException(msg)
{
    public UnexpectedException()
        : this("?")
    {
    }
}