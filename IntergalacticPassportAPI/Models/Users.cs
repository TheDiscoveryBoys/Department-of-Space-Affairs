using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;

namespace IntergalacticPassportAPI.Models
{
    [Table("users")]
    public class Users
    {
        [PrimaryKey(AutoGenerated = false)]
        [Required]
        [Column("google_id")]
        public string? GoogleId{ get; set; }

        [Column("email")]
        public string? Email { get; set; }
        
        [Column("name")]
        public string? Name { get; set; }

        [Column("species")]
        public string? Species { get; set; }

        [Column("planet_of_origin")]
        public string? PlanetOfOrigin { get; set; }

        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }
    }
}
