using Microsoft.EntityFrameworkCore;
using ServiCuentas.Data;
using ServiCuentas.Model;
using ServiCuentas.Shared;

namespace ServiCuentas.Infraestructure.Repository
{
    public class FechaProcesoRepository : IRepository<FechaProceso>
    {
        private readonly AppDBContext _context;

        public FechaProcesoRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Result<FechaProceso>> AddItem(FechaProceso item)
        {
            try
            {
                await _context.FechasProceso.AddAsync(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<FechaProceso>.Failure($"Error en el registro:{ex.Message}");
            }
            return Result<FechaProceso>.Success(item);
        }

        public async Task<Result<FechaProceso>> DeleteItem(int id)
        {
            var resultado = await _context.FechasProceso.FirstOrDefaultAsync(x => x.IdFechaProceso == id);
            if (resultado != null)
            {
                try
                {
                    _context.FechasProceso.Remove(resultado);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    return Result<FechaProceso>.Failure($"Error en la eliminacion:{ex.Message}");
                }
                return Result<FechaProceso>.Success(resultado);
            }
            return Result<FechaProceso>.Failure("No se pudo identificar la fecha de proceso a eliminar");
        }

        public async Task<IEnumerable<FechaProceso>> GetAll()
        {
            var resultado = await _context.FechasProceso.ToListAsync();
            return resultado;
        }

        public Task<IEnumerable<FechaProceso>> GetAllOrderPage(string order, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<FechaProceso>> GetById(int id)
        {
            FechaProceso resultado = new FechaProceso();
            try
            {
                resultado = await _context.FechasProceso.FirstOrDefaultAsync(x => x.IdFechaProceso == id);
            }
            catch (Exception ex)
            {
                return Result<FechaProceso>.Failure($"Error en la recuperacion de la fecha de proceso:{ex.Message}");
            }
            return Result<FechaProceso>.Success(resultado);
        }

        public async Task<Result<FechaProceso>> UpdateItem(FechaProceso item)
        {
            try
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) 
            { 
                return Result<FechaProceso>.Failure($"Error en la modificacion:{ex.Message}"); 
            }
            return Result<FechaProceso>.Success(item);
        }
    }
}
