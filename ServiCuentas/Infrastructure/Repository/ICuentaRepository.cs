using ServiCuentas.Application.DTOs;
using ServiCuentas.Model;
using ServiCuentas.Shared;

namespace ServiCuentas.Infraestructure.Repository
{
    public interface ICuentaRepository : IRepository<Cuenta>
    {
        Task<IEnumerable<Cuenta>> GetAllFilterOrderPage(CuentaFilterDTO filter, string order, int page, int pageSize);
        Task<bool> ExisteNumeroCuenta(Decimal NumeroCuenta);
        Task<bool> ExisteNumeroCuenta(string alias);
        Task<Result<Cuenta>> GetCuentaByNumeroCuenta(decimal NumeroCuenta);
        Task<IEnumerable<Cuenta>> GetCuentasByEmail(string email);
        Task<IEnumerable<Cuenta>> GetCuentasByCelular(string celular);
        Task<Result<Cuenta>> GetCuentaByCvu(string cvu);
        Task<Result<Cuenta>> GetCuentaByAlias(string alias);
    }
}
