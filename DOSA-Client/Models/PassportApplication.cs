namespace DOSA_Client.Models
{
    public record PassportApplication(
        int Id,
        string UserId,
        Status Status,
        DateTime SubmittedAt,
        DateTime? ProcessedAt,
        string? OfficerId
    );
}

