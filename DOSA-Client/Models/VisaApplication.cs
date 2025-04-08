using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSA_Client.Models
{
    public record VisaApplication(
        int id,
        string UserId,
        string DestinationPlanet,
        string TravelReason,
        DateTime StartDate,
        DateTime EndDate,
        int? OfficerId,
        Status Status,
        DateTime SubmittedAt,
        DateTime? ProcessedAt
        
    );
}
