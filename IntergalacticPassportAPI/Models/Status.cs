using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;

namespace IntergalacticPassportAPI.Models
{
	[Table("statuses")]
	public class Status
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[PrimaryKey]
        [Column("id")]
        public int Id { get; set; }

		[Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("reason")]
        public string? Reason { get; set; }


	}
}
