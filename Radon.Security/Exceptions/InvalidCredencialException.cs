namespace Radon.Security.Exceptions;

public class CredentialRejectionException : BaseSecurityException
{
    public CredentialRejectionException()
        : base("Invalid credential") { }

    public CredentialRejectionException(string message)
        : base(message) { }

    public CredentialRejectionException(string message, Exception inner)
        : base(message, inner) { }

    public static void CheckId(long id)
    {
        if (id == long.MaxValue)
        {
            throw new CredentialRejectionException("Bad ID found in credencials.");
        }
    }
}
