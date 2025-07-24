using PJ_P_Installation_Management_System.Data;

namespace PJ_P_Installation_Management_System.Models
{
    public static class UserSeeder
    {
        public static void SeedAdminUser(PJInstallationDbContext context)
        {
            if (!context.Users.Any(u => u.Username == "Admin"))
            {
                var user = new User
                {
                    Username = "Admin",
                    Email = "admin@example.com",
                    Role = "Admin",
                    IsActive = true,
                    Password = "admin123"
                };

                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}
