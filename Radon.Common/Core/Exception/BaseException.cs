namespace Radon.Common.Core.Exception;

public abstract class BaseException : System.Exception
{
    protected BaseException()
        : base("Radon Application Inner Exception")
    {
    }

    protected BaseException(string message)
        : base(message)
    {
    }

    protected BaseException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }
}