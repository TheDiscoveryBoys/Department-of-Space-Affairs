using System.Text.Json.Serialization;

namespace DOSA_Client.Models
{
    public record TravelReason(
        [property: JsonPropertyName("id")] int Id, 
        [property: JsonPropertyName("reason")]string Reason
    );
}