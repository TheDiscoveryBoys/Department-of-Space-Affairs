namespace DOSA_Client.Models
{
    public record User(
        string GoogleId,
        string Email,
        string Name,
        string? Species = null,
        string? PlanetOfOrigin = null,
        string? PrimaryLanguage = null,
        DateTime? DateOfBirth = null
    );
}

