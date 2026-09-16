using System.ComponentModel.DataAnnotations;
using app_Fh_back.Models;
namespace app_Fh_back.Dtos.Categorias;

public class UpdateCategoriaDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Nombre { get; set; } = string.Empty;

    public TipoMovimiento Tipo { get; set; }
}