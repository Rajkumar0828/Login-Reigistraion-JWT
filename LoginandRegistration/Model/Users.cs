using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace LoginandRegistration.Model
{
    public class Users
    {
        [Key]
        public string UsersId { get; set; } = Guid.NewGuid().ToString();
        [Required]

        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public long MobileNumber { get; set; }

        public ICollection<UserRoles> userrole { get; }
    }
}
