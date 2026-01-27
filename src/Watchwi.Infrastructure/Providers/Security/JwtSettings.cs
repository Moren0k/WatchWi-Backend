namespace Watchwi.Infrastructure.Providers.Security;

public sealed record JwtSettings
{
    public const string SectionName = "Jwt";
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string Key { get; init; } = null!;
    public int AccessTokenMinutes { get; init; }
}