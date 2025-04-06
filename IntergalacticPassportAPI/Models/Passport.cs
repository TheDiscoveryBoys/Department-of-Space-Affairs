using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;

namespace IntergalacticPassportAPI.Models
{
    [Table("passport")]
    public class Passport
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int StatusId{ get; set; }

        [Required]
        public DateTime SubmittedAt { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public string? OfficerId { get; set; }
    }
}