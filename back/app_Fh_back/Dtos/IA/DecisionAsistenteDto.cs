namespace app_Finanzas_Hogar.Dtos.IA;

public class DecisionAsistenteDto
{
    public string Accion { get; set; } = string.Empty;

    public ParametrosDecisionDto Parametros { get; set; } = new();
}

public class ParametrosDecisionDto
{
    public string? FechaDesde { get; set; }

    public string? FechaHasta { get; set; }
}