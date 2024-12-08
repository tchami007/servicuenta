using ServiCuentas.Application.DTOs;
using ServiCuentas.Model;

namespace ServiCuentas.Infraestructure.Repository
{
    public interface ICuentaRepository : IRepository<Cuenta>
    {
        Task<IEnumerable<Cuenta>> GetAllFilterOrderPage(CuentaFilterDTO filter, string order, int page, int pageSize);
        Task<bool> ExisteNumeroCuenta(Decimal NumeroCuenta);
    }
}
