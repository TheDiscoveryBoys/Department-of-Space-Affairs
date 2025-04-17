using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DOSA_Client.Models
{
    public record PassportApplication(
    [property: JsonPropertyName("id")] int? Id,
    [property: JsonPropertyName("userId")] string UserId,
    [property: JsonPropertyName("statusId")] int? StatusId,
    [property: JsonPropertyName("submittedAt")] DateTime SubmittedAt,
    [property: JsonPropertyName("processedAt")] DateTime? ProcessedAt,
    [property: JsonPropertyName("officerId")] string? OfficerId,
    [property: JsonPropertyName("officerComment")] string? OfficerComment
    );
}

