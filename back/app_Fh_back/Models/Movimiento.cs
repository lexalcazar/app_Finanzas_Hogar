// Modelo Movimiento
namespace app_Fh_back.Models;
public class Movimiento
{
    // Primary key
    public int Id { get; set; }

    public decimal Cantidad { get; set; }

    public DateOnly Fecha { get; set; }

    public string? Descripcion { get; set; }
    // Relación con la entidad TipoMovimiento
    public TipoMovimiento Tipo { get; set; }
    // Relación con la entidad Categoria (foreign key)
    public int CategoriaId { get; set; }
    // Relación con la entidad Categoria
    public Categoria Categoria { get; set; } = null!;
    // Relación con la entidad Usuario (foreign key)
    public string UsuarioId { get; set; } = string.Empty;
    // Relación con la entidad Usuario 
    public Usuario Usuario { get; set; } = null!;
}