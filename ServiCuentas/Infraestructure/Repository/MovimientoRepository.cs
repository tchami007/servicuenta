using Microsoft.EntityFrameworkCore;
using ServiCuentas.Application.DTOs;
using ServiCuentas.Data;
using ServiCuentas.Model;
using ServiCuentas.Shared;

namespace ServiCuentas.Infraestructure.Repository
{
    public class MovimientoRepository : IMovimientoRepository
    {
        private readonly AppDBContext _context;
        public MovimientoRepository (AppDBContext context)
        {
            _context = context;
        }

        public async Task<Result<Movimiento>> AddItem(Movimiento item)
        {
            try
            {
                await _context.Movimientos.AddAsync(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<Movimiento>.Failure($"Error en la registracion de movimiento:{ex.Message}");
            }
            return Result<Movimiento>.Success(item);
        }

        public async Task<Result<Movimiento>> DeleteItem(int id)
        {
            var resultado = await _context.Movimientos.FirstOrDefaultAsync(x=>x.IdMovimiento==id);
            if (resultado != null)
            {
                try
                {
                    _context.Movimientos.Remove(resultado);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Result<Movimiento>.Failure($"Error en la eliminacion de movimiento:{ex.Message}");
                }
                return Result<Movimiento>.Success(resultado);
            }
            return Result<Movimiento>.Failure("No se pudo identificar el movimiento a eliminar");
        }

        public Task<IEnumerable<Movimiento>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Movimiento>> GetAllMovimientoFilterOrderPage(MovimientoFilterDTO filter, string order, int page, int pageSize)
        {
            // Siempre vamos a recuperar por lo menos por fecha de movimiento
            var query = _context.Movimientos.Where(x => x.FechaMovimiento == filter.FechaMovimiento).AsQueryable();

            // Filtro de Numero de cuenta
            if(filter.NumeroCuenta > 0)
            {
                // Busqueda de la cuenta por Numero de Cuenta
                var cuenta = await _context.Cuentas.AsNoTracking().FirstOrDefaultAsync(x => x.NumeroCuenta == filter.NumeroCuenta);
                if (cuenta != null)
                {
                    //Si existe, aplico el filtro por el id
                    query = query.Where(x => x.IdCuenta == cuenta.IdCuenta);
                }
                
            }

            // Filtro de Comprobante
            if (filter.NumeroComprobante > 0)
            { 
                query = query.Where(x=>x.NumeroComprobante == filter.NumeroComprobante);
            }

            // Filtro de Contrasiento
            if (!string.IsNullOrEmpty(filter.Contrasiento))
            {
                query = query.Where(x=>x.Contrasiento==filter.Contrasiento);
            }

            // Filtro de Codigo de movimiento
            if (filter.CodigoMovimiento > 0)
            {
                query = query.Where(x=>x.CodigoMovimiento==filter.CodigoMovimiento);   
            }

            // filtro de importe
            if (filter.Importe > 0)
            {
                query = query.Where(x=>x.Importe==filter.Importe); 
            }

            var resultado = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new Movimiento { 
                    IdMovimiento = m.IdMovimiento,
                    FechaReal = m.FechaReal,
                    FechaMovimiento = m.FechaMovimiento,
                    IdCuenta = m.IdCuenta,
                    NumeroComprobante = m.NumeroComprobante,
                    Contrasiento = m.Contrasiento,
                    CodigoMovimiento = m.CodigoMovimiento,
                    Importe = m.Importe,
                    BaseImponible = m.BaseImponible,
                    Alicuota = m.Alicuota,
                    SaldoAnterior = m.SaldoAnterior
                }).ToListAsync();

            return resultado;
        }

        public Task<IEnumerable<Movimiento>> GetAllOrderPage(string order, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Movimiento>> GetById(int id)
        {
            Movimiento? resultado = new Movimiento();
            try
            {
                resultado = await _context.Movimientos.FirstOrDefaultAsync(x => x.IdMovimiento == id);
                if (resultado != null)
                {
                    return Result<Movimiento>.Success(resultado);
                }
                else
                {
                    return Result<Movimiento>.Failure("No se pudo identificar el movimiento");
                }

            }
            catch (Exception ex)
            { 
                return Result<Movimiento>.Failure($"Error en la recuperaicon del movimiento:{ex.Message}");
            }
        }

        public async Task<Result<Movimiento>> UpdateItem(Movimiento item)
        {
            try
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<Movimiento>.Failure($"Error en la modificacion del movimiento:{ex.Message}");
            }
            return Result<Movimiento>.Success(item);
        }
    }
}
