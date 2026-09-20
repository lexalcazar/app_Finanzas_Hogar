namespace app_Fh_back.Dtos.Movimientos;

public class EvolucionMensualDto
{
    public int Anio { get; set; }

    public int Mes { get; set; }

    public decimal Ingresos { get; set; }

    public decimal Gastos { get; set; }

    public decimal Balance { get; set; }
}