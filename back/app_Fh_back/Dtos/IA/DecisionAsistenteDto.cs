namespace app_Fh_back.Dtos.IA;

public class DecisionAsistenteDto
{
    public string Accion { get; set; } = string.Empty;

    public ParametrosDecisionDto Parametros { get; set; } = new();
}

public class ParametrosDecisionDto
{
    public string? FechaDesde { get; set; }

    public string? FechaHasta { get; set; }

    public string? Tipo { get; set; }
}