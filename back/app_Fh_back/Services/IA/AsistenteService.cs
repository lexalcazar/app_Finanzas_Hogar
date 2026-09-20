using System.Text.Json;
using app_Fh_back.Dtos.IA;
using app_Fh_back.Dtos.Movimientos;
using app_Fh_back.Models;
namespace app_Fh_back.Services.IA;

public class AsistenteService : IAsistenteService
{
    private readonly ILmStudioService _lmStudioService;
    private readonly MovimientoService _movimientoService;

    public AsistenteService(
        ILmStudioService lmStudioService,
        MovimientoService movimientoService)
    {
        _lmStudioService = lmStudioService;
        _movimientoService = movimientoService;
    }

    public async Task<ChatResponseDto> ProcesarMensajeAsync(
        string usuarioId,
        string mensaje)
    {
        var decisionJson = await _lmStudioService.EnviarPromptAsync(
            ObtenerPromptRouter(),
            mensaje
        );

        var opcionesJson = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var decision = JsonSerializer.Deserialize<DecisionAsistenteDto>(
            decisionJson,
            opcionesJson
        );
        Console.WriteLine($"DECISION IA: {decisionJson}");
        if (decision is null)
        {
            return new ChatResponseDto
            {
                Respuesta = "No he podido interpretar la petición."
            };
        }

        switch (decision.Accion)
        {
            case "obtener_resumen_periodo":
                return await ProcesarResumenPeriodo(
                    usuarioId,
                    mensaje,
                    decision
                );
            case "obtener_resumen_por_categoria":
                return await ProcesarResumenPorCategoria(
                usuarioId,
                mensaje,
                decision
                );
            case "responder_directamente":
                return await ResponderDirectamente(mensaje);

            default:
                return new ChatResponseDto
                {
                    Respuesta = "No he podido determinar qué operación realizar."
                };
        }
    }

    private async Task<ChatResponseDto> ProcesarResumenPeriodo(
        string usuarioId,
        string preguntaOriginal,
        DecisionAsistenteDto decision)
    {
        if (!DateOnly.TryParse(
                decision.Parametros.FechaDesde,
                out var fechaDesde) ||
            !DateOnly.TryParse(
                decision.Parametros.FechaHasta,
                out var fechaHasta))
        {
            return new ChatResponseDto
            {
                Respuesta = "No he podido interpretar correctamente las fechas."
            };
        }

        var resumen = await _movimientoService
            .ObtenerResumenPeriodoAsync(
                usuarioId,
                fechaDesde,
                fechaHasta
            );

        var datos = $"""
            Fecha desde: {resumen.FechaDesde}
            Fecha hasta: {resumen.FechaHasta}
            Ingresos: {resumen.Ingresos}
            Gastos: {resumen.Gastos}
            Balance: {resumen.Balance}
            """;

        var respuesta = await _lmStudioService.EnviarPromptAsync(
            """
            Eres un asistente de finanzas domésticas.

            Responde en español, de forma clara y breve.

            Utiliza exclusivamente los datos financieros proporcionados.
            No inventes cantidades.
            No vuelvas a calcular los importes.
            """,
            $"""
            Pregunta del usuario:
            {preguntaOriginal}

            Datos reales obtenidos de la aplicación:
            {datos}
            """
        );

        return new ChatResponseDto
        {
            Respuesta = respuesta
        };
    }

    private async Task<ChatResponseDto> ResponderDirectamente(
        string mensaje)
    {
        var respuesta = await _lmStudioService.EnviarPromptAsync(
            """
            Eres un asistente integrado en una aplicación
            de finanzas domésticas.

            Responde en español y de forma breve.
            Si la pregunta requiere datos financieros personales
            que no se te han proporcionado, no los inventes.
            """,
            mensaje
        );

        return new ChatResponseDto
        {
            Respuesta = respuesta
        };
    }

    private static string ObtenerPromptRouter()
    {
        return """
            Eres el router de un asistente de finanzas domésticas.

            Tu trabajo NO es responder la pregunta del usuario.
            Tu trabajo es decidir qué acción debe ejecutar la aplicación.

            Acciones disponibles:

            1. obtener_resumen_periodo
            Utilízala cuando el usuario pregunte por ingresos,
            gastos o balance dentro de un intervalo de fechas.

            Parámetros:
            - fechaDesde: YYYY-MM-DD
            - fechaHasta: YYYY-MM-DD

            2. obtener_resumen_por_categoria
            Utilízala cuando el usuario quiera saber cómo se distribuyen
            sus ingresos o gastos por categorías, qué categorías tienen
            mayor importe o cuánto corresponde a cada categoría.

            Parámetros:
            - fechaDesde: YYYY-MM-DD o null
            - fechaHasta: YYYY-MM-DD o null
            - tipo: "Ingreso", "Egreso" o null

            Para obtener_resumen_por_categoria:

            El campo "tipo" SIEMPRE debe estar presente.

            Si el usuario pregunta por:
            - gasto
            - gastos
            - gasté
            - gastar
            - compras
            - pagos
            - en qué se va el dinero

            tipo DEBE ser exactamente "Egreso".

            Si el usuario pregunta por:
            - ingreso
            - ingresos
            - cobré
            - sueldo
            - salario
            - nómina
            - dinero recibido

            tipo DEBE ser exactamente "Ingreso".

            Si el usuario menciona conjuntamente ingresos y gastos,
            el parámetro "tipo" DEBE ser null.

            Ejemplo:
            "¿Cómo se repartieron mis ingresos y gastos en septiembre?"
            → tipo: null

            Solo utiliza null si el usuario pide expresamente
            analizar conjuntamente ingresos y egresos o gastos.

            Nunca omitas el campo "tipo".
            
            3. responder_directamente

            Utilízala únicamente cuando no sea necesario
            consultar datos financieros del usuario.

            Responde EXCLUSIVAMENTE con JSON válido.
            No uses markdown.
            No escribas texto antes ni después.

            Formato:

            {
              "accion": "nombre_accion",
              "parametros": {
                "fechaDesde": null,
                "fechaHasta": null,
                "tipo": null
              }
            }
            """;
    }

    private async Task<ChatResponseDto> ProcesarResumenPorCategoria(
        string usuarioId,
        string preguntaOriginal,
        DecisionAsistenteDto decision)
    {
        DateOnly? fechaDesde = null;
        DateOnly? fechaHasta = null;
        TipoMovimiento? tipo = null;

        if (!string.IsNullOrWhiteSpace(decision.Parametros.FechaDesde))
        {
            if (!DateOnly.TryParse(
                decision.Parametros.FechaDesde,
                out var fechaDesdeParseada))
            {
                return new ChatResponseDto
                {
                    Respuesta = "No he podido interpretar correctamente la fecha inicial."
                };
            }

            fechaDesde = fechaDesdeParseada;
        }

        if (!string.IsNullOrWhiteSpace(decision.Parametros.FechaHasta))
        {
            if (!DateOnly.TryParse(
                    decision.Parametros.FechaHasta,
                    out var fechaHastaParseada))
            {
                return new ChatResponseDto
                {
                    Respuesta = "No he podido interpretar correctamente la fecha final."
                };
            }
        

            fechaHasta = fechaHastaParseada;
        }
    

        if (fechaDesde.HasValue &&
            fechaHasta.HasValue &&
            fechaDesde > fechaHasta)
        {
            return new ChatResponseDto
            {
                Respuesta = "La fecha inicial no puede ser posterior a la fecha final."
            };
        
        }

        if (!string.IsNullOrWhiteSpace(decision.Parametros.Tipo))
        {
            if (!Enum.TryParse<TipoMovimiento>(
                    decision.Parametros.Tipo,
                    true,
                    out var tipoParseado))
            {
                return new ChatResponseDto
                {
                    Respuesta = "No he podido interpretar correctamente el tipo de movimiento."
                };
            }
        

            tipo = tipoParseado;
        }
        if (tipo is null)
        {
            var texto = preguntaOriginal.ToLowerInvariant();

            var hablaDeGastos =
                texto.Contains("gast") ||
                texto.Contains("compra") ||
                texto.Contains("pago") ||
                texto.Contains("egreso");
            
            var hablaDeIngresos =
                texto.Contains("ingres") ||
                texto.Contains("cobr") ||
                texto.Contains("sueldo") ||
                texto.Contains("nómina") ||
                texto.Contains("nomina");
            if (hablaDeGastos && hablaDeIngresos)
            {
                tipo = null;
            }
            else if (tipo is null && hablaDeGastos)
            {
                tipo = TipoMovimiento.Egreso;
            }
            else if (tipo is null && hablaDeIngresos)
            {
                tipo = TipoMovimiento.Ingreso;
            }
        }

        

        var filtro = new FiltroResumenCategoriaDto
        {
            FechaDesde = fechaDesde,
            FechaHasta = fechaHasta,
            Tipo = tipo
        };

        var resumen = await _movimientoService
            .ObtenerResumenPorCategoriaAsync(
                usuarioId,
                filtro
            );
        Console.WriteLine(
                $"TIPO INTERPRETADO: {tipo}"
            );

        Console.WriteLine(
            $"RESUMEN CATEGORIAS: {JsonSerializer.Serialize(resumen)}"
        );
        
        var ingresos = resumen
            .Where(r => r.Tipo == TipoMovimiento.Ingreso)
            .Select(r => $"- {r.CategoriaNombre}: {r.Total} EUR");

        var egresos = resumen
            .Where(r => r.Tipo == TipoMovimiento.Egreso)
            .Select(r => $"- {r.CategoriaNombre}: {r.Total} EUR");

        var datos = $"""
            INGRESOS:
            {string.Join("\n", ingresos)}

            GASTOS / EGRESOS:
            {string.Join("\n", egresos)}
            """;

        var respuesta = await _lmStudioService.EnviarPromptAsync(
            """
            Eres un asistente de finanzas domésticas.

            Responde en español, de forma clara y breve.

            Los datos están separados explícitamente en INGRESOS y GASTOS / EGRESOS.

            No confundas nunca un ingreso con un gasto.
            Una categoría situada bajo INGRESOS es un ingreso.
            Una categoría situada bajo GASTOS / EGRESOS es un gasto.

            Utiliza exclusivamente los datos proporcionados por la aplicación.
            No inventes categorías ni cantidades.
            No calcules porcentajes si no se proporcionan.
            No vuelvas a calcular los importes.
            Utiliza euros (€), nunca dólares ($).

            Si no hay resultados, indícalo claramente.
            """,
            $"""
            Pregunta del usuario:
            {preguntaOriginal}

            Resumen real por categorías obtenido de la aplicación:
            {datos}
            """
        );

        return new ChatResponseDto
        {
            Respuesta = respuesta
        };
    }
}