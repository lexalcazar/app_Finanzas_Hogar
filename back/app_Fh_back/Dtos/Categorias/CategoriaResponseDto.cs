using app_Fh_back.Models;

namespace app_Fh_back.Dtos.Categorias;

public class CategoriaResponseDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public TipoMovimiento Tipo { get; set; }
}