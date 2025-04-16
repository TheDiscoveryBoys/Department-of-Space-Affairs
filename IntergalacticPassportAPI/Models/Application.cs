using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntergalacticPassportAPI.Models
{
    public abstract class Application
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [PrimaryKey]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public string UserId { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        [Required]
        [Column("submitted_at")]
        public DateTime SubmittedAt { get; set; }

        [Column("processed_at")]
        public DateTime? ProcessedAt { get; set; }

        [Column("officer_id")]
        public string? OfficerId { get; set; }

        [Column("officer_comment")]
        public string? OfficerComment { get; set; }
    }
}
