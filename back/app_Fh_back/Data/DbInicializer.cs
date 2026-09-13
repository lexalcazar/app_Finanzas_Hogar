using app_Fh_back.Models;
using Microsoft.AspNetCore.Identity;

namespace app_Fh_back.Data;

public static class DbInitializer
{
    public static async Task InicializarAsync(IServiceProvider serviceProvider)
    {
        var roleManager =
            serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var userManager =
            serviceProvider.GetRequiredService<UserManager<Usuario>>();

        var configuration =
            serviceProvider.GetRequiredService<IConfiguration>();

        // Crear roles si no existen
        string[] roles = { "User", "Admin" };

        foreach (var rol in roles)
        {
            if (!await roleManager.RoleExistsAsync(rol))
            {
                await roleManager.CreateAsync(new IdentityRole(rol));
            }
        }

        // Datos del administrador
        var adminEmail = configuration["Admin:Email"];
        var adminPassword = configuration["Admin:Password"];

        if (string.IsNullOrEmpty(adminEmail) ||
            string.IsNullOrEmpty(adminPassword))
        {
            return;
        }

        // Buscar si ya existe
        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            admin = new Usuario
            {
                UserName = adminEmail,
                Email = adminEmail,
                Nombre = "Administrador"
            };

            var resultado =
                await userManager.CreateAsync(admin, adminPassword);

            if (resultado.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}