
using AutoMapper;
using ServiCuentas.Application.DTOs;
using ServiCuentas.Infraestructure.Repository;
using ServiCuentas.Model;
using ServiCuentas.Shared;

namespace ServiCuentas.Application.Services
{
    public interface IMovimientoService
    {
        Task<Result<MovimientoDTO>> AddMovimiento(MovimientoDTO movimiento);
        Task<Result<MovimientoDTO>> DeleteMovimiento(int id);
        Task<Result<MovimientoDTO>> UpdateMovimiento(int id, MovimientoDTO movimiento);
        Task<Result<MovimientoDTO>> GetMovimientoById (int id);
        Task<IEnumerable<MovimientoDTO>> GetAllMovimiento (MovimientoFilterDTO filter, string order, int page, int pageSize);
    }
    public class MovimientoService : IMovimientoService
    {
        private readonly IMovimientoRepository _repository;
        private readonly IMapper _mapper;

        public MovimientoService(IMovimientoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<MovimientoDTO>> AddMovimiento(MovimientoDTO movimiento)
        {
            var movimientoNuevo = _mapper.Map<Movimiento>(movimiento);
            var resultado = await _repository.AddItem(movimientoNuevo);
            if (!resultado._success)
            {
                return Result<MovimientoDTO>.Failure($"Error en la registracion: {resultado._errorMessage}");
            }
            var retorno = _mapper.Map<MovimientoDTO>(resultado._value);
            return Result<MovimientoDTO>.Success(retorno);
        }

        public async Task<Result<MovimientoDTO>> DeleteMovimiento(int id)
        {
            var resultado = await _repository.DeleteItem(id);
            if (!resultado._success)
            {
                return Result<MovimientoDTO>.Failure($"Error en la eliminacion de movimiento: {resultado._errorMessage}");
            }

            var retorno = _mapper.Map<MovimientoDTO>(resultado._value);
            return Result<MovimientoDTO>.Success(retorno);

        }

        public async Task<IEnumerable<MovimientoDTO>> GetAllMovimiento(MovimientoFilterDTO filter, string order, int page, int pageSize)
        {
            var resultado = await _repository.GetAllMovimientoFilterOrderPage(filter, order, page, pageSize);
            var retorno = _mapper.Map<IEnumerable<MovimientoDTO>>(resultado);
            return retorno;
        }

        public async Task<Result<MovimientoDTO>> GetMovimientoById(int id)
        {
            var resultado = await _repository.GetById(id);
            if (!resultado._success)
            {
                return Result<MovimientoDTO>.Failure($"Error en la recuperacion: {resultado._errorMessage}");
            }
            var retorno = _mapper.Map<MovimientoDTO>(resultado._value);
            return Result<MovimientoDTO>.Success(retorno );
        }

        public async Task<Result<MovimientoDTO>> UpdateMovimiento(int id, MovimientoDTO movimiento)
        {
            var movimientoModificado = _mapper.Map<Movimiento>(movimiento);
            var resultado = await _repository.UpdateItem(movimientoModificado);
            if (!resultado._success)
            {
                return Result<MovimientoDTO>.Failure($"Error en la modificacion: {resultado._errorMessage}");
            }
            var retorno = _mapper.Map<MovimientoDTO>(resultado._value);
            return Result<MovimientoDTO>.Success(retorno);
        }
    }
}
