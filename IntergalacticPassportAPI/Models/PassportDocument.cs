﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;

namespace IntergalacticPassportAPI.Models
{
    [Table("passport_application_documents")]
    public class PassportDocument 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [PrimaryKey]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("filename")]
        public string Filename { get; set; }

        [Required]
        [Column("s3_url")]
        public string S3Url {get; set;}

        [Required]
        [Column("passport_application_id")]
        public int PassportApplicationId { get; set; }
    }

}