using ECommerce.Domain.Constants;
using ECommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Infrastructure.Persistence.Seeding;

public class IdentitySeeder(
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager,
    IConfiguration config) : IDataSeeder
{
    public int Order => 0;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedRolesAsync(cancellationToken);
        await SeedSuperAdminAsync(cancellationToken);
    }

    private async Task SeedRolesAsync(CancellationToken ct)
    {
        if (await roleManager.Roles.AnyAsync(ct))
            return;

        foreach(var roleName in Roles.All)
        {
            await roleManager.CreateAsync(new ApplicationRole(roleName)
            {
                Description = $"{roleName} System Role"
            });
        }
    }

    private async Task SeedSuperAdminAsync(CancellationToken ct)
    {
        var section = config.GetSection("Seed:SuperAdmin");
        var superAdminEmail = section["Email"];
        var superAdminPassword = section["Password"];

        if (string.IsNullOrWhiteSpace(superAdminEmail) || string.IsNullOrWhiteSpace(superAdminPassword))
            return;
        if(await userManager.Users.AnyAsync(u => u.Email == superAdminEmail, ct))
            return;

        var user = new ApplicationUser
        {
            UserName = superAdminEmail,
            Email = superAdminEmail,
            EmailConfirmed = true,
            DisplayName = section["DisplayName"] ?? "Super Admin"
        };

        var result = await userManager.CreateAsync(user, superAdminPassword);
        if (result.Succeeded) 
            await userManager.AddToRoleAsync(user, Roles.SuperAdmin);
    }
}
