
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;

namespace IntergalacticPassportAPI.Models
{
    [Table("roles")]
    public class Roles
    {
        [PrimaryKey]
        [Column("id")]
        public int Id { get; set; }
        [Column("role")]
        public string Role { get; set; }
    }
}
