using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntergalacticPassportAPI.Models
{

    [Table("visa_applications")]

    public class Visa : Application
    {

        [Required]
        [Column("destination_planet")]
        public string DestinationPlanet { get; set; }

        [Required]
        [Column("travel_reason")]
        public string TravelReason { get; set; }

        [Required]
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; }
    }

}
