using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntergalacticPassportAPI.Models
{
    public class UserRoles{

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [PrimaryKey]
        [Column("id")]
        public int Id {get; set;}
        [Column("user_id")]
        public string UserId {get; set;}
        [Column("role_id")]
        public int RoleId {get; set;}
    }
}
