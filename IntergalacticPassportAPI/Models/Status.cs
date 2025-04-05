using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;

namespace IntergalacticPassportAPI.Models
{
	[Table("status")]
	public class Status
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
        public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string? Reason { get; set; }


	}
}
