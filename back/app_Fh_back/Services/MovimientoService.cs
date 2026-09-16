using app_Fh_back.Data;
using app_Fh_back.Dtos.Movimientos;
using Microsoft.EntityFrameworkCore;

namespace app_Fh_back.Services;

public class MovimientoService
{
    private readonly ApplicationDbContext _context;

    public MovimientoService(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Movimientos
    public async Task<List<MovimientoResponseDto>> ObtenerTodosAsync(
        string usuarioId)
    {
        return await _context.Movimientos
            .AsNoTracking()
            .Where(m => m.UsuarioId == usuarioId)
            .OrderByDescending(m => m.Fecha)
            .Select(m => new MovimientoResponseDto
            {
                Id = m.Id,
                Cantidad = m.Cantidad,
                Fecha = m.Fecha,
                Descripcion = m.Descripcion,
                Tipo = m.Tipo,
                CategoriaId = m.CategoriaId,
                Categoria = m.Categoria.Nombre
            })
            .ToListAsync();
    }
}