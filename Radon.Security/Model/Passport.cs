using Radon.Security.Enums;

namespace Radon.Security.Model;

public class Passport(string token, DateTime expr, RoleType role = RoleType.Anonymous)
{
    public string Token { get; init; } = token;
    public RoleType Role { get; init; } = role;
    public DateTime Expiration { get; init; } = expr;
}
