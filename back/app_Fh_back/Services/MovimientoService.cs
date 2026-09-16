using app_Fh_back.Data;
using app_Fh_back.Dtos.Movimientos;
using Microsoft.EntityFrameworkCore;
using app_Fh_back.Models;

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
    //Get: api/Movimientos {id} 
        public async Task<MovimientoResponseDto?> ObtenerPorIdAsync(
        int id,
        string usuarioId)
        {
        return await _context.Movimientos
            .AsNoTracking()
            .Where(m => m.Id == id && m.UsuarioId == usuarioId)
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
            .FirstOrDefaultAsync();
    }
    // post : api/Movimientos

    public async Task<MovimientoResponseDto?> CrearAsync(
        CreateMovimientoDto dto,
        string usuarioId)
    {
        var categoria = await _context.Categorias
            .FindAsync(dto.CategoriaId);

        if (categoria == null)
        {
            return null;
        }

        var movimiento = new Movimiento
        {
            Cantidad = dto.Cantidad,
            Descripcion = dto.Descripcion,
            Fecha = dto.Fecha,
            CategoriaId = categoria.Id,
            Tipo = categoria.Tipo,
            UsuarioId = usuarioId
        };

        _context.Movimientos.Add(movimiento);

        await _context.SaveChangesAsync();

        return new MovimientoResponseDto
        {
            Id = movimiento.Id,
            Cantidad = movimiento.Cantidad,
            Descripcion = movimiento.Descripcion,
            Fecha = movimiento.Fecha,
            Tipo = movimiento.Tipo,
            CategoriaId = categoria.Id,
            Categoria = categoria.Nombre
        };
    }
    // put : api/Movimientos/{id}
    public async Task<MovimientoResponseDto?> ActualizarAsync(
        int id,
        UpdateMovimientoDto dto,
        string usuarioId)
    {
        var movimiento = await _context.Movimientos
            .FirstOrDefaultAsync(m =>
                m.Id == id &&
                m.UsuarioId == usuarioId);

        if (movimiento == null)
        {
            return null;
        }

        var categoria = await _context.Categorias
            .FindAsync(dto.CategoriaId);

        if (categoria == null)
        {
            return null;
        }

        movimiento.Cantidad = dto.Cantidad;
        movimiento.Descripcion = dto.Descripcion;
        movimiento.Fecha = dto.Fecha;
        movimiento.CategoriaId = categoria.Id;
        movimiento.Tipo = categoria.Tipo;

        await _context.SaveChangesAsync();

        return new MovimientoResponseDto
        {
            Id = movimiento.Id,
            Cantidad = movimiento.Cantidad,
            Descripcion = movimiento.Descripcion,
            Fecha = movimiento.Fecha,
            Tipo = movimiento.Tipo,
            CategoriaId = categoria.Id,
            Categoria = categoria.Nombre
        };
    }
    // delete : api/Movimientos/{id}
    public async Task<bool> EliminarAsync(
        int id,
        string usuarioId)
    {
        var movimiento = await _context.Movimientos
            .FirstOrDefaultAsync(m =>
                m.Id == id &&
                m.UsuarioId == usuarioId);

        if (movimiento == null)
        {
            return false;
        }

        _context.Movimientos.Remove(movimiento);

        await _context.SaveChangesAsync();

        return true;
    }
}

