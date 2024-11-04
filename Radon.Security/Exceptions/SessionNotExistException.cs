using Radon.Common.Core.Exception;

namespace Radon.Security.Exceptions;

public class SessionNotExistException()
    : BaseException("Token is valid but session does not exist.") { }
