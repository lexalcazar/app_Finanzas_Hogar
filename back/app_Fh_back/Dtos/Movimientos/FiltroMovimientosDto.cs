using app_Fh_back.Models;

namespace app_Fh_back.Dtos.Movimientos;

public class FiltroMovimientosDto
{
    public DateOnly? FechaDesde { get; set; }

    public DateOnly? FechaHasta { get; set; }

    public TipoMovimiento? Tipo { get; set; }

    public int? CategoriaId { get; set; }

    public string? Busqueda { get; set; }
}