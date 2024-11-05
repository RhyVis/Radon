using Radon.Security.Enums;

namespace Radon.Security.Model;

public class Passport(string token, string username, DateTime expr)
{
    public string Token { get; init; } = token;
    public string Username { get; init; } = username;
    public DateTime Expiration { get; init; } = expr;
}
