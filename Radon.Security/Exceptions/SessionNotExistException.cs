namespace Radon.Security.Exceptions;

public class SessionNotExistException()
    : BaseSecurityException("Token is valid but session does not exist.");
