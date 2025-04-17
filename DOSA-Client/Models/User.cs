using System.Text.Json.Serialization;

namespace DOSA_Client.Models
{
    public record User(
        [property: JsonPropertyName("googleId")] string google_id,
        [property: JsonPropertyName("email")] string email,
        [property: JsonPropertyName("name")] string name,
        [property: JsonPropertyName("species")] string? species = null,
        [property: JsonPropertyName("planetOfOrigin")] string? planet_of_origin = null,
        [property: JsonPropertyName("dateOfBirth")] DateTime? date_of_birth = null
    );
}

