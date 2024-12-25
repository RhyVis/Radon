namespace Radon.Security.Model;

public record Passport(string Token, long UserId, PassportExtra Extra);

public record PassportExtra(string ImageToken);
