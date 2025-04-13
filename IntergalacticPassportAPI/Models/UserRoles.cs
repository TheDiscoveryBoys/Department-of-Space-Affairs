using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper;

namespace IntergalacticPassportAPI.Models
{
    [Table("user_roles")]
    public class UserRoles
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [PrimaryKey]
        [Column("id")]
        public int? Id {get; set;}
        [Column("user_id")]
        public string UserId { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }
    }
}
