using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StormChasersGroupProject2.Models;

namespace StormChasersGroupProject2.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager)
        {
            using (var context = new ApplicationDbContext(
              serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();

                //look for users
                if(context.Users.Any())
                {
                    return;
                }

                var user = new ApplicationUser
                {
                    UserName = "testuser@example.com",
                    Email = "testuser@example.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, "Test@1234");

            }
        }
    }
}
