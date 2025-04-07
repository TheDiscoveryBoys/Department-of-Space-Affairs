namespace DOSA_Client.Models
{
    public enum ApplicationType
    {
        Passport,
        Visa
    }

    // This class serves as a collation of relevant data for a user's application. (For the User to see)
    public record Application(
        Status Status,
        DateTime SubmittedAt,
        ApplicationType ApplicationType,
        DateTime? ProcessedAt = null
    );
}
