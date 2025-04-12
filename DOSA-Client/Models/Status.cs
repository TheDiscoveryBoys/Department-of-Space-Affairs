using System.Text.Json.Serialization;

namespace DOSA_Client.Models
{
    public record Status(
    [property: JsonPropertyName("id")] int? Id, 
    [property: JsonPropertyName("name")]string Name, 
    [property: JsonPropertyName("reason")]string? Reason = null
    );
}

