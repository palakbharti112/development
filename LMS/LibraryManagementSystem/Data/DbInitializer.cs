using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.Migrate();

                if (!context.Roles.Any())
                {
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    roleManager.CreateAsync(new IdentityRole(Role.Librarian)).Wait();
                    roleManager.CreateAsync(new IdentityRole(Role.Member)).Wait();
                }

                if (!context.Users.Any())
                {
                    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var user = new ApplicationUser { UserName = "admin", Email = "admin@example.com" };
                    userManager.CreateAsync(user, "Admin@123").Wait();
                    userManager.AddToRoleAsync(user, Role.Librarian).Wait();
                }
            }
        }
    }
}
