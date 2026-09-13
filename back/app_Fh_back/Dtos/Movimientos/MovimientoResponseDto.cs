using app_Fh_back.Models;
namespace app_Fh_back.Dtos.Movimientos;
public class MovimientoResponseDto
{
    public int Id { get; set; }
    public decimal Cantidad { get; set; }
    public string? Descripcion { get; set; } 
    public DateOnly Fecha { get; set; }
    public TipoMovimiento Tipo { get; set; }
    public int CategoriaId { get; set; }
    public string? Categoria { get; set; } = string.Empty;
}