namespace Radon.Security.Enums;

[Flags]
public enum RoleType
{
    Anonymous = 1,
    Guest = 2,
    User = 4,
    Admin = 8,
}
