using ServiCuentas.Infraestructure.Repository;
using ServiCuentas.Model;
using ServiCuentas.Shared;

namespace ServiCuentas.Application.Services
{
    public interface IFechaProcesoService
    {
        Task<Result<FechaProceso>> AddFechaProceso(FechaProceso fecha);
        Task<Result<FechaProceso>> DeleteFechaProceso(int id);
        Task<Result<FechaProceso>> UpdateFechaProceso(int id, FechaProceso fecha);
        Task<Result<FechaProceso>> GetFechaProcesoById(int id);
        Task<IEnumerable<FechaProceso>> GetAll();
    }
    public class FechaProcesoService : IFechaProcesoService
    {
        private readonly IRepository<FechaProceso> _repository; 

        public FechaProcesoService(IRepository<FechaProceso> repository)
        {
            _repository = repository;
        }

        public async Task<Result<FechaProceso>> AddFechaProceso(FechaProceso fecha)
        {
            var resultado = await _repository.AddItem(fecha);
            return resultado;
        }

        public async Task<Result<FechaProceso>> DeleteFechaProceso(int id)
        {
            var resultado = await _repository.DeleteItem(id);
            return resultado;
        }

        public async Task<IEnumerable<FechaProceso>> GetAll()
        {
            var resultado = await _repository.GetAll();
            return resultado;
        }

        public async Task<Result<FechaProceso>> GetFechaProcesoById(int id)
        {
            var resultado = await _repository.GetById(id);
            return resultado;
        }

        public async Task<Result<FechaProceso>> UpdateFechaProceso(int id, FechaProceso fecha)
        {
            if (id != fecha.IdFechaProceso)
            {
                return Result<FechaProceso>.Failure("Los identificadores no se corresponden");
            }
           var resultado = await _repository.UpdateItem(fecha);
            return resultado;
        }
    }
}
