namespace Radon.Security.Exceptions;

public class UserNotFoundException(string name)
    : BaseSecurityException($"User {name} not found")
{
}