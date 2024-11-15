using FreeSql.DataAnnotations;
using Radon.Data.Entity;
using Radon.Security.Enums;

namespace Radon.Security.Data.Entity;

/// <summary>
/// User entity for login and authorization.
/// </summary>
[Table(Name = "security_user")]
public class User : BaseIdEntity
{
    public string Username { get; init; } = null!;
    public string Password { get; set; } = null!;
    public RoleType Role { get; set; } = RoleType.Anonymous;
}
