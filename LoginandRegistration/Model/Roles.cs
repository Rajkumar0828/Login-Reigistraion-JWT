using System.ComponentModel.DataAnnotations;

namespace LoginandRegistration.Model
{
    public class Roles
    {
        [Key]

        public string? RolesId { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(40)]
        public string? RoleName { get; set; }

        public ICollection<UserRoles> Userrole { get; } = new List<UserRoles>();

    }
}
