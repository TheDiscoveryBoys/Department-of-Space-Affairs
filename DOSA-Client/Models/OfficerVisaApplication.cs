namespace DOSA_Client.Models{
    public record OfficerVisaApplication(
        VisaApplication VisaApplication,
        User Applicant
    );
}