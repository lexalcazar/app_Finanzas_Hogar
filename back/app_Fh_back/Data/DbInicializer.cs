using Microsoft.AspNetCore.Identity;

namespace app_Fh_back.Data;

public static class DbInitializer
{
    public static async Task CrearRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager =
            serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { "User", "Admin" };

        foreach (var rol in roles)
        {
            if (!await roleManager.RoleExistsAsync(rol))
            {
                await roleManager.CreateAsync(new IdentityRole(rol));
            }
        }
    }
}