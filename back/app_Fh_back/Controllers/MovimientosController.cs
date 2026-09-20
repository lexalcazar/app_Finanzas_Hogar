using System.Security.Claims;
using app_Fh_back.Dtos.Movimientos;
using app_Fh_back.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace app_Fh_back.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MovimientosController : ControllerBase
{
    private readonly MovimientoService _service;

    public MovimientosController(MovimientoService service)
    {
        _service = service;
    }
    // GET: api/Movimientos
    [HttpGet]
    public async Task<ActionResult<List<MovimientoResponseDto>>> ObtenerTodos(
        [FromQuery] FiltroMovimientosDto filtro)
    {
        var usuarioId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(usuarioId))
        {
            return Unauthorized();
        }
    

        if (filtro.FechaDesde.HasValue &&
            filtro.FechaHasta.HasValue &&
            filtro.FechaDesde > filtro.FechaHasta)
        {
            return BadRequest(new
            {
                message = "La fecha desde no puede ser posterior a la fecha hasta"
            });
        }

        var movimientos =
            await _service.ObtenerTodosAsync(usuarioId, filtro);

        return Ok(movimientos);
    }
    // GET: api/Movimientos/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<MovimientoResponseDto>> ObtenerPorId(int id)
    {
        var usuarioId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(usuarioId))
        {
            return Unauthorized();
        }

        var movimiento =
            await _service.ObtenerPorIdAsync(id, usuarioId);

        if (movimiento == null)
        {
            return NotFound(new
            {
                message = "Movimiento no encontrado"
            });
        }
    

        return Ok(movimiento);
    }
    // POST: api/Movimientos
    [HttpPost]
    public async Task<ActionResult<MovimientoResponseDto>> Crear(
        [FromBody] CreateMovimientoDto dto)
    {
        var usuarioId =
        User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(usuarioId))
        {
            return Unauthorized();
        }

        var movimiento =
            await _service.CrearAsync(dto, usuarioId);

        if (movimiento == null)
        {
            return BadRequest(new
            {
                message = "La categoría indicada no existe"
            });
        }
    

        return CreatedAtAction(
        nameof(ObtenerPorId),
        new { id = movimiento.Id },
        movimiento);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MovimientoResponseDto>> Actualizar(
        int id,
        [FromBody] UpdateMovimientoDto dto)
    {
        var usuarioId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(usuarioId))
        {
            return Unauthorized();
        }

        var movimiento =
            await _service.ActualizarAsync(id, dto, usuarioId);

        if (movimiento == null)
        {
            return NotFound(new
            {
                message = "Movimiento no encontrado o categoría no válida"
            });
        }
    

        return Ok(movimiento);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var usuarioId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(usuarioId))
        {
            return Unauthorized();
        }

        var eliminado =
            await _service.EliminarAsync(id, usuarioId);

        if (!eliminado)
        {
            return NotFound(new
            {
                message = "Movimiento no encontrado"
            });
        }
    

        return NoContent();
    }
    [HttpGet("resumen")]
    public async Task<ActionResult<ResumenMovimientosDto>> ObtenerResumen(
        [FromQuery] DateOnly? fechaHasta)
        {
            var usuarioId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(usuarioId))
        {
            return Unauthorized();
        }

        var fechaCalculo =
            fechaHasta ?? DateOnly.FromDateTime(DateTime.Today);

        var resumen =
            await _service.ObtenerResumenAsync(
                usuarioId,
                fechaCalculo);

        return Ok(resumen);
    }
}    
    
