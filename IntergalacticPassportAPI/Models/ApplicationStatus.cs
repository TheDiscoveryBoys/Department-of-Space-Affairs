using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;

namespace IntergalacticPassportAPI.Models
{
	[Table("application_statuses")]
	public class Status
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[PrimaryKey]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        //public Status(String name, String reason){
        //    this.Name = name;
        //    this.Reason = reason;
        //}

        //public Status(int id, String name, String reason){
        //    this.Id = id;
        //    this.Name = name;
        //    this.Reason= reason;
        //}
        //public Status(){}

	}
}
