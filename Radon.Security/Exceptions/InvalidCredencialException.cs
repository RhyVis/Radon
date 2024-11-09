namespace Radon.Security.Exceptions;

public class InvalidCredentialException : BaseSecurityException
{
    public InvalidCredentialException()
        : base("Invalid credential") { }

    public InvalidCredentialException(string message)
        : base(message) { }

    public InvalidCredentialException(string message, Exception inner)
        : base(message, inner) { }
}
