namespace DOSA_Client.Models
{
    public enum ApplicationType
    {
        Passport,
        Visa
    }

    public class Application
    {
        public Status Status { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public ApplicationType ApplicationType { get; set; }

        public Application(Status status, DateTime submittedAt, ApplicationType applicationType)
        {
            Status = status;
            SubmittedAt = submittedAt;
            ApplicationType = applicationType;
        }

        public Application(Status status, DateTime submittedAt, DateTime processedAt, ApplicationType applicationType)
        {
            Status = status;
            SubmittedAt = submittedAt;
            ProcessedAt = processedAt;
            ApplicationType = applicationType;
        }
    }
}
