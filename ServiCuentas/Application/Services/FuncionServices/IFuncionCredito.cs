using ServiCuentas.Application.DTOs;
using ServiCuentas.Shared;

namespace ServiCuentas.Application.Services.FuncionServices
{
    public interface IFuncionCredito
    {
        Task<Result<MovimientoDTO>> Ejecutar(FuncionRequestAsientoDTO funcion);
        Task<Result<MovimientoDTO>> Contrasentar(FuncionRequestDTO funcion);

    }
}
