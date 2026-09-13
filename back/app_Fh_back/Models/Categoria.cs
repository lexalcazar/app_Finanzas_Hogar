// Modelo de la entidad Categoria
public class Categoria
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    // Relación con la entidad TipoMovimiento
    public TipoMovimiento Tipo { get; set; }
    // Relación con la entidad Movimiento
    public ICollection<Movimiento> Movimientos { get; set; }
        = new List<Movimiento>();
}