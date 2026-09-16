using app_Fh_back.Dtos.Categorias;
using app_Fh_back.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app_Fh_back.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriasController : ControllerBase
{
    private readonly CategoriaService _service;

    public CategoriasController(CategoriaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoriaResponseDto>>> ObtenerTodas()
    {
        var categorias =
            await _service.ObtenerTodasAsync();

        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaResponseDto>> ObtenerPorId(int id)
    {
        var categoria =
            await _service.ObtenerPorIdAsync(id);

        if (categoria == null)
        {
            return NotFound();
        }

        return Ok(categoria);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<CategoriaResponseDto>> Crear(
        [FromBody] CreateCategoriaDto dto)
    {
        var categoria =
            await _service.CrearAsync(dto);

        if (categoria == null)
        {
            return Conflict(new
            {
                message = "La categoría ya existe"
            });
        }

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = categoria.Id },
            categoria);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] UpdateCategoriaDto dto)
    {
        var actualizado =
            await _service.ActualizarAsync(id, dto);

        if (!actualizado)
        {
            return NotFound();
        }

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado =
            await _service.EliminarAsync(id);

        if (!eliminado)
        {
            return NotFound();
        }

        return NoContent();
    }
}