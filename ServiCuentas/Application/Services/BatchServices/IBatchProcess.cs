using ServiCuentas.Shared;

namespace ServiCuentas.Application.Services.BatchServices
{
    public interface IBatchProcess<T>
    {
        Task<Result<string>> IngresoMasivoProcess(T config);
        Task<Result<string>> CalculoProcess(T config);
        Task<Result<string>> DevengamientoInteresProcess(T config);
        Task<Result<string>> LiquidacionInteresProcess(T config);
        Task<Result<string>> ComisionesProcess(T config);
        Task<Result<string>> CierreFechaProcess(T config);
        Task<Result<string>> AperturaFechaProcess(T config);
    }
}
