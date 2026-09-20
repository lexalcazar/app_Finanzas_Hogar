using app_Fh_back.Models;
namespace app_Fh_back.Dtos.Movimientos;
public class ResumenMovimientosDto
{
    public DateOnly FechaCalculo { get; set; }

    public decimal TotalIngresos { get; set; }

    public decimal TotalGastos { get; set; }

    public decimal Saldo { get; set; }
}