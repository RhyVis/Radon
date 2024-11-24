namespace Radon.Core.Model.Extra;

public record UsernamePair
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}
