using app_Fh_back.Models;
using System.ComponentModel.DataAnnotations;
namespace app_Fh_back.Dtos.Categorias;

public class CreateCategoriaDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Nombre { get; set; } = string.Empty;

    public TipoMovimiento Tipo { get; set; }
}