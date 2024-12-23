using ServiCuentas.Infraestructure.Repository;
using ServiCuentas.Model;
using ServiCuentas.Shared;

namespace ServiCuentas.Application.Services
{
    public interface INumeracionService
    {
        Task<Result<Numeracion>> AddNumeracion(Numeracion numeracion);
        Task<Result<Numeracion>> DeleteNumeracion(int id);
        Task<Result<Numeracion>> UpdateNumeracion(int id, Numeracion numeracion);
        Task<Result<Numeracion>> GetNumeracionById(int id);
        Task<Result<Numeracion>> GetNumeracionByDescripcion(string descripcion);
        Task<IEnumerable<Numeracion>> GetAllNumeracion();
    }
    public class NumeracionService : INumeracionService
    {
        private readonly INumeracionRepository _repository;

        public NumeracionService(INumeracionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Numeracion>> AddNumeracion(Numeracion numeracion)
        {
            var resultado = await _repository.AddItem(numeracion);
            return resultado;
        }

        public async Task<Result<Numeracion>> DeleteNumeracion(int id)
        {
            var resultado = await _repository.DeleteItem(id);
            return resultado;
        }

        public async Task<IEnumerable<Numeracion>> GetAllNumeracion()
        {
            var resultado = await _repository.GetAll();
            return resultado;
        }

        public async Task<Result<Numeracion>> GetNumeracionByDescripcion(string descripcion)
        {
            var resultado = await _repository.GetAll();
            var retorno = resultado.Where(x=>x.Descripcion == descripcion).FirstOrDefault();
            if (retorno == null)
            {
                return Result<Numeracion>.Failure($"No se encuentra la numeracion: {descripcion}");
            }

            retorno.UltimoNumero = retorno.UltimoNumero + 1;
            await _repository.UpdateItem(retorno);

            return Result<Numeracion>.Success(retorno);
        }

        public async Task<Result<Numeracion>> GetNumeracionById(int id)
        {
            var resultado = await _repository.GetById(id);
            return resultado;
        }

        public async Task<Result<Numeracion>> UpdateNumeracion(int id, Numeracion numeracion)
        {
            if (id != numeracion.IdNumeracion)
            {
                return Result<Numeracion>.Failure("Los identificadores no se corresponden");
            }
            var resultado = await _repository.UpdateItem(numeracion);
            return resultado;
        }
    }
}
