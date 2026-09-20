using app_Fh_back.Models;

namespace app_Fh_back.Dtos.Movimientos;

public class ResumenPorCategoriaDto
{
    public int CategoriaId { get; set; }

    public string CategoriaNombre { get; set; } = string.Empty;

    public TipoMovimiento Tipo { get; set; }

    public decimal Total { get; set; }
}