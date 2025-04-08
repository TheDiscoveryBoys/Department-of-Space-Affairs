using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntergalacticPassportAPI.Models
{
    public class UserRoles{

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("id")]
        public int Id {get; set;}
        [Column("UserId")]
        public string UserId {get; set;}
        [Column("RoleId")]
        public int RoleId {get; set;}
    }
}
