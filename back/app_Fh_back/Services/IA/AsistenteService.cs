using System.Text.Json;
using app_Fh_back.Dtos.IA;
using app_Fh_back.Dtos.Movimientos;
using app_Fh_back.Models;
namespace app_Fh_back.Services.IA;
// Servicio que procesa las peticiones del asistente de IA
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
// Procesa el mensaje del usuario y devuelve la respuesta del asistente de IA
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
            
            case "obtener_evolucion_mensual":
                return await ProcesarEvolucionMensual(
                usuarioId,
                mensaje,
                decision
                );
            
            case "buscar_movimientos":
                return await ProcesarBusquedaMovimientos(
                usuarioId,
                mensaje,
                decision
                );

            case "obtener_resumen_mensual":
                return await ProcesarResumenMensual(
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
// Procesa la acción "obtener_resumen_periodo" del asistente de IA
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
    // Genera el prompt para el router del asistente de IA
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
            
            3. obtener_evolucion_mensual

            Utilízala cuando el usuario quiera conocer cómo han evolucionado
            sus ingresos, gastos o balance a lo largo de varios meses,
            comparar meses o ver una tendencia mensual.

            Parámetros:
            - fechaDesde: YYYY-MM-DD
            - fechaHasta: YYYY-MM-DD

            Ejemplos de preguntas:
            - "¿Cómo han evolucionado mis gastos este año?"
            - "Muéstrame mis ingresos y gastos de los últimos seis meses"
            - "Compara mis finanzas entre junio y septiembre"
            - "¿Estoy gastando más cada mes?"
            
            Ejemplo de respuesta JSON para la pregunta:

            Usuario:
            "¿Cómo evolucionaron mis ingresos y gastos entre junio y septiembre de 2026?"

            Respuesta:
            {
                "accion": "obtener_evolucion_mensual",
                "parametros": {
                "fechaDesde": "2026-06-01",
                "fechaHasta": "2026-09-30",
                "tipo": null
                }
            }
            
            4. buscar_movimientos

            Utilízala cuando el usuario quiera localizar, listar o consultar
            movimientos concretos.

            Ejemplos:
            - "Busca los movimientos de Amazon"
            - "¿Qué pagos hice en agosto?"
            - "Enséñame mis gastos entre el 10 y el 20 de septiembre"
            - "¿Tengo algún movimiento relacionado con gasolina?"

            Parámetros:
            - fechaDesde: YYYY-MM-DD o null
            - fechaHasta: YYYY-MM-DD o null
            - tipo: "Ingreso", "Egreso" o null
            - busqueda: texto que debe buscarse en la descripción, o null

            Ejemplo de respuesta JSON para la pregunta:
            Usuario:
            "Busca mis gastos de Amazon durante septiembre de 2026"

            Respuesta:
            {
                "accion": "buscar_movimientos",
                "parametros": {
                                "fechaDesde": "2026-09-01",
                                "fechaHasta": "2026-09-30",
                                "tipo": "Egreso",
                                "busqueda": "Amazon"
                            }
            }

            5. obtener_resumen_mensual

            Utilízala cuando el usuario pregunte por el resumen financiero
            de un mes concreto: ingresos totales, gastos totales o balance
            de ese mes.

            Parámetros:
            - mes: número del mes entre 1 y 12
            - anio: año con cuatro cifras

            Ejemplos:
            - "Hazme un resumen de septiembre de 2026"
            - "¿Cuál fue mi balance en agosto de 2026?"
            - "¿Cuánto ingresé y gasté en julio?"
            
            Ejemplo de respuesta JSON para la pregunta:
            Usuario:
            "Hazme un resumen de septiembre de 2026"

            Respuesta:
            {
                "accion": "obtener_resumen_mensual",
                "parametros": {
                                "fechaDesde": null,
                                "fechaHasta": null,
                                "tipo": null,
                                "busqueda": null,
                                "mes": 9,
                                "anio": 2026
                            }
                        
            }

            6. responder_directamente

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
                "tipo": null,
                "busqueda": null,
                "mes": null,
                "anio": null
              }
            }
            """;
    }
    // Procesa la acción "obtener_resumen_por_categoria" del asistente de IA
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
    // Procesa la acción "obtener_evolucion_mensual" del asistente de IA
    private async Task<ChatResponseDto> ProcesarEvolucionMensual(
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
    

        if (fechaDesde > fechaHasta)
        {
            return new ChatResponseDto
            {
                Respuesta = "La fecha inicial no puede ser posterior a la fecha final."
            };
        }
    

        var evolucion = await _movimientoService
            .ObtenerEvolucionMensualAsync(
                usuarioId,
                fechaDesde,
                fechaHasta
            );

        if (evolucion.Count == 0)
        {
            return new ChatResponseDto
            {
                Respuesta = "No hay movimientos en el periodo indicado."
            };
        }
    

        var datos = string.Join(
            "\n",
            evolucion.Select(e =>
                $"{e.Anio}-{e.Mes:D2}: " +
                $"Ingresos {e.Ingresos} EUR | " +
                $"Gastos {e.Gastos} EUR | " +
                $"Balance {e.Balance} EUR"
            )
        );

        var respuesta = await _lmStudioService.EnviarPromptAsync(
            """
            Eres un asistente de finanzas domésticas.

            Responde en español, de forma clara y breve.

            Los datos proporcionados corresponden a la evolución
            financiera mensual real del usuario.

            Ingresos significa dinero recibido.
            Gastos significa dinero gastado.
            Balance es ingresos menos gastos.

            Utiliza exclusivamente los datos proporcionados.
            No inventes cantidades.
            No inventes porcentajes.
            No cambies ingresos por gastos.
            Utiliza euros (€), nunca dólares ($).

            Si describes una tendencia, hazlo únicamente a partir
            de los meses proporcionados.

            Si no hay datos en un mes, indícalo claramente. No inventes datos para ese mes.

            No deduzcas tendencias si hay menos de dos meses con datos reales.
            """,
            $"""
            Pregunta del usuario:
            {preguntaOriginal}

            Evolución mensual real:
            {datos}
            """
        );

        return new ChatResponseDto
        {
            Respuesta = respuesta
        };
    
    }
    // Procesa la acción "buscar_movimientos" del asistente de IA
    private async Task<ChatResponseDto> ProcesarBusquedaMovimientos(
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
            if (Enum.TryParse<TipoMovimiento>(
                    decision.Parametros.Tipo,
                    true,
                    out var tipoParseado))
            {
                tipo = tipoParseado;
            
            }
        }

        var filtro = new FiltroMovimientosDto
        {
            FechaDesde = fechaDesde,
            FechaHasta = fechaHasta,
            Tipo = tipo,
            Busqueda = decision.Parametros.Busqueda,
            CategoriaId = null
        };

        var movimientos = await _movimientoService
            .ObtenerTodosAsync(usuarioId, filtro);

        if (movimientos.Count == 0)
        {
            return new ChatResponseDto
            {
                Respuesta = "No he encontrado movimientos que coincidan con esa búsqueda."
            };
        }

        var datos = string.Join(
            "\n",
            movimientos.Select(m =>
                $"- {m.Fecha}: {m.Categoria} | " +
                $"{m.Tipo} | {m.Cantidad} EUR | " +
                $"{m.Descripcion}"
            )
        );

        var respuesta = await _lmStudioService.EnviarPromptAsync(
            """
            Eres un asistente de finanzas domésticas.

            Responde en español y de forma breve.

            Los movimientos proporcionados son datos reales de la aplicación.
            No inventes movimientos, fechas, cantidades ni categorías.
            No añadas movimientos que no aparezcan en los datos.
            Utiliza euros (€), nunca dólares ($).
            Para los egresos, responde con la palabra "Gasto" y para los ingresos, responde con la palabra "Ingreso".

            Si el usuario pide una lista, resume únicamente los movimientos
            proporcionados.
            """,
            $"""
            Pregunta del usuario:
            {preguntaOriginal}

            Movimientos encontrados:
            {datos}
            """
        );

        return new ChatResponseDto
        {
            Respuesta = respuesta
        };
    }

    // Procesa la acción "obtener_resumen_mensual" del asistente de IA
    private async Task<ChatResponseDto> ProcesarResumenMensual(
        string usuarioId,
        string preguntaOriginal,
        DecisionAsistenteDto decision)
    {
        if (!decision.Parametros.Mes.HasValue ||
            !decision.Parametros.Anio.HasValue)
        {
            return new ChatResponseDto
            {
                Respuesta = "No he podido determinar correctamente el mes y el año."
            };
        };
    

        var mes = decision.Parametros.Mes.Value;
        var anio = decision.Parametros.Anio.Value;

        if (mes < 1 || mes > 12)
        {
            return new ChatResponseDto
            {
                Respuesta = "El mes indicado no es válido."
            };
        }
    

        var fechaDesde = new DateOnly(anio, mes, 1);

        var fechaHasta = fechaDesde
            .AddMonths(1)
            .AddDays(-1);

        var resumen = await _movimientoService
            .ObtenerResumenPeriodoAsync(
                usuarioId,
                fechaDesde,
                fechaHasta
            );

        var respuesta = await _lmStudioService.EnviarPromptAsync(
            """
            Eres un asistente de finanzas domésticas.

            Responde en español, de forma clara y breve.

            Los datos proporcionados son datos reales de la aplicación.

            Ingresos significa dinero recibido.
            Gastos significa dinero gastado.
            Balance significa ingresos menos gastos.

            Utiliza exclusivamente los datos proporcionados.
            No inventes cantidades.
            No inventes movimientos.
            No inventes porcentajes.
            Utiliza euros (€), nunca dólares ($).

            Si todos los importes son cero, indica que no existen
            movimientos registrados durante ese mes.
            """,
            $"""
            Pregunta del usuario:
            {preguntaOriginal}

            Mes: {mes}
            Año: {anio}

            Datos reales:
            Ingresos: {resumen.Ingresos} EUR
            Gastos: {resumen.Gastos} EUR
            Balance: {resumen.Balance} EUR
            """
        );

        return new ChatResponseDto
        {
            Respuesta = respuesta
        };
    }

}