using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tugas_Test_EID
{
    [Table("user_login")]
    public class DB_User_Login
    {
        [Key]
        public required string id { get; set; }
        public required string username {get; set;}
        public required string password {get; set;}
    }
}