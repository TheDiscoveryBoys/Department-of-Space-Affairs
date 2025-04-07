using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;

namespace IntergalacticPassportAPI.Models
{

    [Table("visa_applications")]

    public class Visa
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public string UserId { get; set; }

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

        [Required]
        [Column("status_id")]
        public int StatusId { get; set; }

        [Required]
        [Column("submitted_at")]
        public DateTime SubmittedAt { get; set; }

        [Column("processed_at")]
        public DateTime? ProcessedAt { get; set; }

        [Column("officer_id")]

        public string? OfficerId { get; set; }
    }

}
