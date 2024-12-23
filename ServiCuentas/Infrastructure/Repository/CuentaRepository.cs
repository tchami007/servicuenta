                                                                                                                                                                                                                                                                                                                                                                                                using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ServiCuentas.Application.DTOs;
using ServiCuentas.Data;
using ServiCuentas.Model;
using ServiCuentas.Shared;

namespace ServiCuentas.Infraestructure.Repository
{
    public class CuentaRepository : ICuentaRepository
    {
        private readonly AppDBContext _context; 
        private readonly PaginationSettings _pagination;
                                                                                                                                                                                                                                                        
        public CuentaRepository(AppDBContext context, IOptions<PaginationSettings> pagination) 
        { 
            _context = context; 
            _pagination = pagination.Value; 
        }

        public async Task<Result<Cuenta>> AddItem(Cuenta item)
        {
            try
            {
                await _context.Cuentas.AddAsync(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<Cuenta>.Failure($"Error en registro:{ex.Message}");
            }
            return Result<Cuenta>.Success(item);
        }

        public async Task<Result<Cuenta>> DeleteItem(int id)
        {
            var resultado = await _context.Cuentas.FirstOrDefaultAsync(x=>x.IdCuenta==id);
            if (resultado != null)
            {
                try
                {
                    _context.Cuentas.Remove(resultado);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Result<Cuenta>.Failure($"Error en la eliminacion:{ex.Message}");
                }
                return Result<Cuenta>.Success(resultado);
            }
            return Result<Cuenta>.Failure("Cuenta no identificada");
        }

        public async Task<IEnumerable<Cuenta>> GetAll()
        {
            var resultado = await _context.Cuentas.ToListAsync();
            return resultado;
        }

        public async Task<IEnumerable<Cuenta>> GetAllFilterOrderPage(CuentaFilterDTO filter, string order, int page, int pageSize)
        {
            // Recuperacion total inicial
            var query = _context.Cuentas.AsQueryable();

            // Filtro de numero de cuenta
            if (filter.NumeroCuenta > 0 && filter.NumeroCuenta != null)
            {
                query = query.Where(x => x.NumeroCuenta == filter.NumeroCuenta);
            }

            // Filtro de Descripcion
            if (!string.IsNullOrEmpty(filter.Descripcion))
            {
                query = query.Where(x => x.Descripcion == filter.Descripcion);
            }

            // Filtro de Fecha de Alta
            if (filter.FechaAlta != null)
            {
                query = query.Where(x => x.FechaAlta == filter.FechaAlta);
            }
            // Filtro de estado
            if (filter.Estado != null)
            {
                query = query.Where(x => x.Estado == filter.Estado);
            }

            List<Cuenta> resultado = await ApplyOrderRetrieve(order, page, ref pageSize, ref query);

            return resultado;

        }

        private Task<List<Cuenta>> ApplyOrderRetrieve(string order, int page, ref int pageSize, ref IQueryable<Cuenta> query)
        {
            // Ordenamiento
            if (order == "numerocuenta")
            {
                query = query.OrderBy(x => x.NumeroCuenta);
            }
            if (order == "descripcion")
            {
                query = query.OrderBy(x => x.Descripcion);
            }
            if (order == "fechaalta")
            {
                query = query.OrderBy(x => x.FechaAlta);
            }

            /*
            // Recuperacion desde configuracion (si es 0 o negativo)
            if (pageSize <= 0)
            {
                pageSize = _pagination.DefaultPageSize;
            }
            */

            // Resultado
            var resultado = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new Cuenta
                {
                    IdCuenta = c.IdCuenta,
                    Descripcion = c.Descripcion,
                    FechaAlta = c.FechaAlta,
                    NumeroCuenta = c.NumeroCuenta,
                    Capital = c.Capital,
                    Interes = c.Interes,
                    Ajuste = c.Ajuste,
                    DevengadoAcumulado = c.DevengadoAcumulado,
                    Estado = c.Estado,
                    Cvu = c.Cvu,
                    Alias = c.Alias,
                    Email = c.Email,
                    Celular = c.Celular
                }).ToListAsync();

            return resultado;
        }

        public async Task<IEnumerable<Cuenta>> GetAllOrderPage(string order, int page, int pageSize)
        {
            var query = _context.Cuentas.AsQueryable();

            List<Cuenta> resultado = await ApplyOrderRetrieve(order, page, ref pageSize, ref query);

            return resultado;

        }

        public async Task<Result<Cuenta>> GetById(int id)
        {
            Cuenta resultado = new Cuenta();

            try
            {
                resultado = await _context.Cuentas.FirstOrDefaultAsync(x => x.IdCuenta == id);
            }
            catch (Exception ex)
            {
                return Result<Cuenta>.Failure($"Error en la recuperacion:{ex.Message}");
            }
            return Result<Cuenta>.Success(resultado);
        }

        public async Task<Result<Cuenta>> UpdateItem(Cuenta item)
        {
            try
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<Cuenta>.Failure($"Error en la modificacion:{ex.Message}");
            }
            return Result<Cuenta>.Success(item);

        }

        public async Task<bool> ExisteNumeroCuenta(decimal NumeroCuenta)
        {
            var resultado = await _context.Cuentas.AnyAsync(x=>x.NumeroCuenta == NumeroCuenta);
            return resultado;
        }
        public async Task<bool> ExisteNumeroCuenta(string alias)
        {
            var resultado = await _context.Cuentas.AnyAsync(x => x.Alias == alias);
            return resultado;

        }

        public async Task<Result<Cuenta>> GetCuentaByNumeroCuenta(decimal NumeroCuenta)
        {
            Cuenta resultado = new Cuenta();

            try
            {
                resultado = await _context.Cuentas.FirstOrDefaultAsync(x => x.NumeroCuenta== NumeroCuenta);
            }
            catch (Exception ex)
            {
                return Result<Cuenta>.Failure($"Error en la recuperacion:{ex.Message}");
            }
            return Result<Cuenta>.Success(resultado);

        }

        public async Task<IEnumerable<Cuenta>> GetCuentasByEmail(string email)
        {
            var resultado = await _context.Cuentas.Where(x=> x.Email == email).ToListAsync();
            return resultado;
        }

        public async Task<IEnumerable<Cuenta>> GetCuentasByCelular(string celular)
        {
            var resultado = await _context.Cuentas.Where(x => x.Celular == celular).ToListAsync();
            return resultado;
        }

        public async Task<Result<Cuenta>> GetCuentaByCvu(string cvu)
        {
            var resultado = await _context.Cuentas.Where(x => x.Cvu == cvu).ToListAsync();
            Cuenta cuentaRetorno = new Cuenta();
            if (resultado.Count() == 1) 
            {
                cuentaRetorno = resultado.FirstOrDefault();
                return Result<Cuenta>.Success(cuentaRetorno);
            }

            return Result<Cuenta>.Failure($"No se pudo encontrar la cuenta del cvu: {cvu}");
        }

        public async Task<Result<Cuenta>> GetCuentaByAlias(string alias)
        {
            var resultado = await _context.Cuentas.Where(x => x.Alias == alias).ToListAsync();
            Cuenta cuentaRetorno = new Cuenta();
            if (resultado.Count() == 1)
            {
                cuentaRetorno = resultado.FirstOrDefault();
                return Result<Cuenta>.Success(cuentaRetorno);
            }

            return Result<Cuenta>.Failure($"Error: No se pudo encontrar la cuenta del alias: {alias}");

        }
    }
}
