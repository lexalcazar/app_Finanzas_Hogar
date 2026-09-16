using System.ComponentModel.DataAnnotations;

namespace app_Fh_back.Dtos.Movimientos;

public class UpdateMovimientoDto
{
    [Range(typeof(decimal), "0.01", "999999999")]
    public decimal Cantidad { get; set; }

    [StringLength(250)]
    public string? Descripcion { get; set; }

    public DateOnly Fecha { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoriaId { get; set; }
}