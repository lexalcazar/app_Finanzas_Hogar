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
        string usuarioId,
        FiltroMovimientosDto filtro)
    {
        var query = _context.Movimientos
            .AsNoTracking()
            .Where(m => m.UsuarioId == usuarioId)
            .AsQueryable();

        if (filtro.FechaDesde.HasValue)
        {
            query = query.Where(m =>
                m.Fecha >= filtro.FechaDesde.Value);
        }

        if (filtro.FechaHasta.HasValue)
        {
            query = query.Where(m =>
                m.Fecha <= filtro.FechaHasta.Value);
        }

        if (filtro.Tipo.HasValue)
        {
            query = query.Where(m =>
                m.Tipo == filtro.Tipo.Value);
        }

        if (filtro.CategoriaId.HasValue)
        {
            query = query.Where(m =>
                m.CategoriaId == filtro.CategoriaId.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
        {
            var texto = filtro.Busqueda.Trim();

            query = query.Where(m =>
                m.Descripcion != null &&
                EF.Functions.ILike(m.Descripcion, $"%{texto}%"));
        }

        return await query
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

    // GET: api/Movimientos/resumen
        public async Task<ResumenMovimientosDto> ObtenerResumenAsync(
        string usuarioId,
        DateOnly fechaHasta)
    {
        var movimientos = _context.Movimientos
            .Where(m =>
                m.UsuarioId == usuarioId &&
                m.Fecha <= fechaHasta);

        var totalIngresos = await movimientos
            .Where(m => m.Tipo == TipoMovimiento.Ingreso)
            .SumAsync(m => m.Cantidad);

        var totalGastos = await movimientos
            .Where(m => m.Tipo == TipoMovimiento.Egreso)
            .SumAsync(m => m.Cantidad);

        return new ResumenMovimientosDto
        {
            FechaCalculo = fechaHasta,
            TotalIngresos = totalIngresos,
            TotalGastos = totalGastos,
            Saldo = totalIngresos - totalGastos
        };
    }

    // GET: api/Movimientos/ResumenPorCategorias
    public async Task<List<ResumenPorCategoriaDto>> ObtenerResumenPorCategoriaAsync(
        string usuarioId,
        FiltroResumenCategoriaDto filtro)
    {
        var query = _context.Movimientos
            .AsNoTracking()
            .Where(m => m.UsuarioId == usuarioId)
            .AsQueryable();

        if (filtro.FechaDesde.HasValue)
        {
            query = query.Where(m =>
                m.Fecha >= filtro.FechaDesde.Value);
        
        }

        if (filtro.FechaHasta.HasValue)
        {
            query = query.Where(m =>
                m.Fecha <= filtro.FechaHasta.Value);
        }

        if (filtro.Tipo.HasValue)
        {
            query = query.Where(m =>
                m.Tipo == filtro.Tipo.Value);
        }

    

        return await query
            .GroupBy(m => new
            {
                m.CategoriaId,
                CategoriaNombre = m.Categoria.Nombre,
                m.Tipo
            })
            .Select(g => new ResumenPorCategoriaDto
            {
                CategoriaId = g.Key.CategoriaId,
                CategoriaNombre = g.Key.CategoriaNombre,
                Tipo = g.Key.Tipo,
                Total = g.Sum(m => m.Cantidad)
            })
            .OrderByDescending(r => r.Total)
            .ToListAsync();
    }

    // GET: api/Movimientos/EvolucionMensual
    public async Task<List<EvolucionMensualDto>> ObtenerEvolucionMensualAsync(
        string usuarioId,
        DateOnly fechaDesde,
        DateOnly fechaHasta)
    {
        return await _context.Movimientos
            .AsNoTracking()
            .Where(m =>
                m.UsuarioId == usuarioId &&
                m.Fecha >= fechaDesde &&
                m.Fecha <= fechaHasta)
            .GroupBy(m => new
            {
                Anio = m.Fecha.Year,
                Mes = m.Fecha.Month
            })
            .Select(g => new EvolucionMensualDto
            {
                Anio = g.Key.Anio,
                Mes = g.Key.Mes,

                Ingresos = g.Sum(m =>
                    m.Tipo == TipoMovimiento.Ingreso
                        ? m.Cantidad
                        : 0),

                Gastos = g.Sum(m =>
                    m.Tipo == TipoMovimiento.Egreso
                        ? m.Cantidad
                        : 0),

                Balance =
                    g.Sum(m =>
                        m.Tipo == TipoMovimiento.Ingreso
                            ? m.Cantidad
                            : 0)
                    -
                    g.Sum(m =>
                        m.Tipo == TipoMovimiento.Egreso
                            ? m.Cantidad
                            : 0)
            })
            .OrderBy(r => r.Anio)
            .ThenBy(r => r.Mes)
            .ToListAsync();
    }

    // GET: api/Movimientos/ResumenPeriodo
        public async Task<ResumenPeriodoDto> ObtenerResumenPeriodoAsync(
        string usuarioId,
        DateOnly fechaDesde,
        DateOnly fechaHasta)
    {
        var movimientos = _context.Movimientos
            .AsNoTracking()
            .Where(m =>
                m.UsuarioId == usuarioId &&
                m.Fecha >= fechaDesde &&
                m.Fecha <= fechaHasta);

        var ingresos = await movimientos
            .Where(m => m.Tipo == TipoMovimiento.Ingreso)
        .   SumAsync(m => m.Cantidad);

        var gastos = await movimientos
            .Where(m => m.Tipo == TipoMovimiento.Egreso)
            .SumAsync(m => m.Cantidad);

        return new ResumenPeriodoDto
        {
            FechaDesde = fechaDesde,
            FechaHasta = fechaHasta,
            Ingresos = ingresos,
            Gastos = gastos,
            Balance = ingresos - gastos
        };
    }
}


