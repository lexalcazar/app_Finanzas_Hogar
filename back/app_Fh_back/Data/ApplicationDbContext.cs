using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using app_Fh_back.Models;
namespace app_Fh_back.Data;

public class ApplicationDbContext : IdentityDbContext<Usuario>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Movimiento> Movimientos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
}