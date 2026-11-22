
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ENN_AAR_Sanitaryware.Data
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var configuration = scope.ServiceProvider
                .GetRequiredService<IConfiguration>();

            const string role = "Admin";
            const string username = "admin";
            const string email = "inpreetk07@gmail.com";

            if (!await roleManager.RoleExistsAsync(role))
            {
                var roleResult =
                    await roleManager.CreateAsync(new IdentityRole(role));

                if (!roleResult.Succeeded)
                    throw new Exception("Could not create Admin role.");
            }

            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = true
                };

                var password = configuration["AdminSeed:Password"];

                if (string.IsNullOrWhiteSpace(password))
                    throw new Exception("Admin password is missing.");

                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("; ",
                        result.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
