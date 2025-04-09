
using DOSA_Client.Models;

namespace DOSA_Client.Models{
    public record OfficerPassportApplication(
        PassportApplication PassportApplication,
        User Applicant,
        List<ApplicationDocument> ApplicationDocuments
    );
}