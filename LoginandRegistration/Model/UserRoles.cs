using System.ComponentModel.DataAnnotations;

namespace LoginandRegistration.Model
{
    public class UserRoles
    {

        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Users User { get; set; }
        public Roles Role { get; set; }

    }
}
