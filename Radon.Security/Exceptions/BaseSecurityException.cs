using Radon.Common.Core.Exception;

namespace Radon.Security.Exceptions;

public abstract class BaseSecurityException(string msg) : BaseException(msg) { }
