using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;

namespace Dot.Net.WebApi.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        RoleManager<IdentityRole> roleManager =
            services.GetRequiredService<RoleManager<IdentityRole>>();
        UserManager<User> userManager =
            services.GetRequiredService<UserManager<User>>();

        string[] roles = { "Admin", "User" };
        foreach (string role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        string adminUserName = "admin";
        if (await userManager.FindByNameAsync(adminUserName) is null)
        {
            User admin = new User
            {
                UserName = adminUserName,
                Email = "admin@findexium.com",
                Fullname = "Administrateur"
            };
            IdentityResult result = await userManager.CreateAsync(admin, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}