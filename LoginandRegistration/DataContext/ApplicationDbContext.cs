using LoginandRegistration.Model;
using Microsoft.EntityFrameworkCore;

namespace LoginandRegistration.DataContext
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Users> User { get; set; }
        public DbSet<UserRoles> UserRole { get; set; }
        public DbSet<Roles> Roles { get; set; }

    }
}
