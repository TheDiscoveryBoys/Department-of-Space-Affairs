using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;

namespace IntergalacticPassportAPI.Models
{
    [Table("passport_applications")]
    public class Passport
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
<<<<<<< HEAD
=======

>>>>>>> 7d4a6e4cd5551efdc959a5758855192d918aaa95
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public string UserId { get; set; }

        [Required]
        [Column("status_id")]
        public int StatusId{ get; set; }

        [Required]
        [Column("submitted_at")]
        public DateTime SubmittedAt { get; set; }

        [Column("processed_at")]
        public DateTime? ProcessedAt { get; set; }

        [Column("officer_id")]
<<<<<<< HEAD
=======
      
>>>>>>> 7d4a6e4cd5551efdc959a5758855192d918aaa95
        public string? OfficerId { get; set; }
    }
}