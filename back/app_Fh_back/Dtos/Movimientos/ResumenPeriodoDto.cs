namespace app_Fh_back.Dtos.Movimientos;

public class ResumenPeriodoDto
{
    public DateOnly FechaDesde { get; set; }

    public DateOnly FechaHasta { get; set; }

    public decimal Ingresos { get; set; }

    public decimal Gastos { get; set; }

    public decimal Balance { get; set; }
}