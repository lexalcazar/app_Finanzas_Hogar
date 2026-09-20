using app_Fh_back.Models;

namespace app_Fh_back.Dtos.Movimientos;

public class FiltroResumenCategoriaDto
{
    public DateOnly? FechaDesde { get; set; }

    public DateOnly? FechaHasta { get; set; }

    public TipoMovimiento? Tipo { get; set; }
}