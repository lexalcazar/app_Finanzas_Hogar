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

    [HttpGet]
    public async Task<ActionResult<List<MovimientoResponseDto>>> ObtenerTodos()
    {
        var usuarioId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(usuarioId))
        {
            return Unauthorized();
        }

        var movimientos =
            await _service.ObtenerTodosAsync(usuarioId);

        return Ok(movimientos);
    }
}