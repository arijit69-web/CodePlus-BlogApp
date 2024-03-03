using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePlus.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "3b1dd6f9-82ea-43b3-abb7-09d82d4297a1";
            var writerRoleId = "2800be2f-6ae1-47ec-9133-4e4cec58090d";




            // Create Reader & Writer Role
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId

                },
                 new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId

                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            // Create an Admin User

            var adminUserId = "b105b288-ded6-44e2-9e58-1c4e17eb59d6";

            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@codeplus.com",
                Email = "admin@codeplus.com",
                NormalizedEmail = "admin@codeplus.com".ToUpper(),
                NormalizedUserName = "admin@codeplus.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            builder.Entity<IdentityUser>().HasData(admin);

            // Give Roles to Admin

            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }



            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
