using System.ComponentModel;

namespace Radon.Security.Enums;

[Flags]
public enum RoleType
{
    [Description("Anonymous")]
    Anonymous = 1,

    [Description("Guest")]
    Guest = 2,

    [Description("User")]
    User = 4,

    [Description("Admin")]
    Admin = 8,
}
