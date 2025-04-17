namespace DOSA_Client.Models
{
    

    // This class serves as a collation of relevant data for a user's application. (For the User to see)
    public record Application(
        Status Status,
        DateTime SubmittedAt,
        String ApplicationType,
        String? OfficerComment = null,
        DateTime? ProcessedAt = null
    );
}
