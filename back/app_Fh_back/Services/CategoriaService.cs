using app_Fh_back.Data;
using app_Fh_back.Dtos.Categorias;
using app_Fh_back.Models;
using Microsoft.EntityFrameworkCore;

namespace app_Fh_back.Services;
public class CategoriaService
{
    private readonly ApplicationDbContext _context;

    public CategoriaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoriaResponseDto>> ObtenerTodasAsync()
    {
        return await _context.Categorias
            .AsNoTracking()
            .OrderBy(c => c.Tipo)
            .ThenBy(c => c.Nombre)
            .Select(c => new CategoriaResponseDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Tipo = c.Tipo
            })
            .ToListAsync();
    }

    public async Task<CategoriaResponseDto?> ObtenerPorIdAsync(int id)
    {
        return await _context.Categorias
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CategoriaResponseDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Tipo = c.Tipo
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CategoriaResponseDto?> CrearAsync(CreateCategoriaDto dto)
    {
        var existe = await _context.Categorias
            .AnyAsync(c =>
                c.Nombre.ToLower() == dto.Nombre.ToLower() &&
                c.Tipo == dto.Tipo);

        if (existe)
        {
            return null;
        }

        var categoria = new Categoria
        {
            Nombre = dto.Nombre.Trim(),
            Tipo = dto.Tipo
        };

        _context.Categorias.Add(categoria);

        await _context.SaveChangesAsync();

        return new CategoriaResponseDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            Tipo = categoria.Tipo
        };
    }

    public async Task<bool> ActualizarAsync(
        int id,
        UpdateCategoriaDto dto)
    {
        var categoria =
            await _context.Categorias.FindAsync(id);

        if (categoria == null)
        {
            return false;
        }

        categoria.Nombre = dto.Nombre.Trim();
        categoria.Tipo = dto.Tipo;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var categoria =
            await _context.Categorias.FindAsync(id);

        if (categoria == null)
        {
            return false;
        }

        _context.Categorias.Remove(categoria);

        await _context.SaveChangesAsync();

        return true;
    }
}