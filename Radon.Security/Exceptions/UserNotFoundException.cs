using Radon.Common.Core.Exception;

namespace Radon.Security.Exceptions;

public class UserNotFoundException(string name) : BaseException($"User {name} not found") { }
