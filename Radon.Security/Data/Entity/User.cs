using FreeSql.DataAnnotations;
using JetBrains.Annotations;
using Radon.Data.Entity;
using Radon.Security.Enums;
using Radon.Security.Model;

namespace Radon.Security.Data.Entity;

/// <summary>
///     User entity for login and authorization.
/// </summary>
[Table(Name = "security_user")]
public class User : BaseIdEntity
{
    public string Username { get; init; } = null!;
    public string Password { get; set; } = null!;
    public RoleType Role { get; set; } = RoleType.Anonymous;

    [JsonMap] public UserExtra Extra { get; set; } = new();
}

[UsedImplicitly(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.WithMembers)]
public class UserExtra
{
    public string ImageToken { get; set; } = string.Empty;

    public PassportExtra MapToPassportExtra()
    {
        return new PassportExtra(ImageToken);
    }
}
