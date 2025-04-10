using System.Text.Json.Serialization;

namespace DOSA_Client.Models
{
    public record VisaApplication(
        [property: JsonPropertyName("id")] int? Id,
        [property: JsonPropertyName("userId")] string UserId,
        [property: JsonPropertyName("statusId")] int? StatusId,
        [property: JsonPropertyName("destinationPlanet")] string DestinationPlanet,
        [property: JsonPropertyName("travelReason")] string TravelReason,
        [property: JsonPropertyName("startDate")] DateTime StartDate,
        [property: JsonPropertyName("endDate")] DateTime? EndDate,
        [property: JsonPropertyName("submittedAt")] DateTime SubmittedAt,
        [property: JsonPropertyName("processedAt")] DateTime? ProcessedAt,
        [property: JsonPropertyName("officerId")] string? OfficerId
    );
}
