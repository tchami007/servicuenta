using ServiCuentas.Application.DTOs;
using ServiCuentas.Shared;

namespace ServiCuentas.Application.Services.FuncionServices
{
    public interface IFuncionDebito
    {
        Task<Result<MovimientoDTO>> Ejecutar(FuncionRequestAsientoDTO funcion);
        Task<Result<MovimientoDTO>> Contrasentar(FuncionRequestDTO funcion);
    }
}
