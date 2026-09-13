// Modelo Usuario

using Microsoft.AspNetCore.Identity;

public class Usuario : IdentityUser
{
    public string Nombre { get; set; } = string.Empty;

    // Relación con la entidad Movimiento
    public ICollection<Movimiento> Movimientos { get; set; }
        = new List<Movimiento>();
}

