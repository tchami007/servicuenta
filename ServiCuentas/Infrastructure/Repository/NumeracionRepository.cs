using Microsoft.EntityFrameworkCore;
using ServiCuentas.Data;
using ServiCuentas.Model;
using ServiCuentas.Shared;

namespace ServiCuentas.Infraestructure.Repository
{
    public class NumeracionRepository : INumeracionRepository
    {
        private readonly AppDBContext _context;
        public NumeracionRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Result<Numeracion>> AddItem(Numeracion item)
        {
            try
            {
                await _context.Numeraciones.AddAsync(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<Numeracion>.Failure($"Error en el registro:{ex.Message}");
            }
            return Result<Numeracion>.Success(item);
        }

        public async Task<Result<Numeracion>> DeleteItem(int id)
        {
            var resultado = await _context.Numeraciones.FirstOrDefaultAsync(x => x.IdNumeracion == id);
            if (resultado != null)
            {
                try
                {
                    _context.Numeraciones.Remove(resultado);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Result<Numeracion>.Failure($"Error en la eliminacion:{ex.Message}");
                }
                return Result<Numeracion>.Success(resultado);
            }
            return Result<Numeracion>.Failure("No se pudo identificar la numeracion a eliminar");
        }

        public async Task<IEnumerable<Numeracion>> GetAll()
        {
           var resultado = await _context.Numeraciones.ToListAsync();
           return resultado;
        }

        public Task<IEnumerable<Numeracion>> GetAllOrderPage(string order, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Numeracion>> GetByDescripcion(string descripcion)
        {
            try
            {
                var resultado = await _context.Numeraciones.FirstOrDefaultAsync(x => x.Descripcion == descripcion);

                resultado.UltimoNumero += 1;

                var actualizacion = await UpdateItem(resultado);
                if (!actualizacion._success)
                {
                    return Result<Numeracion>.Failure("Fallo en la Actualizacion del comprobante");
                }

                return Result<Numeracion>.Success(resultado);
            }
            catch (Exception ex)
            { 
                return Result<Numeracion>.Failure($"{ex.Message}");
            }
            
        }

        public async Task<Result<Numeracion>> GetById(int id)
        {
            Numeracion? resultado = new Numeracion();
            try
            {
                resultado = await _context.Numeraciones.FirstOrDefaultAsync(x => x.IdNumeracion == id);
            }
            catch (Exception ex)
            {
                return Result<Numeracion>.Failure($"Error en la recuperacion de la numeracion:{ex.Message}");
            }
            if (resultado != null)
            {
                return Result<Numeracion>.Success(resultado);
            }
            return Result<Numeracion>.Failure("No se pudo identificar la numeracion");
        }

        public async Task<Result<Numeracion>> UpdateItem(Numeracion item)
        {
            try
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<Numeracion>.Failure($"Error en la modificacion:{ex.Message}");
            }
            return Result<Numeracion>.Success(item);
        }

    }
}
