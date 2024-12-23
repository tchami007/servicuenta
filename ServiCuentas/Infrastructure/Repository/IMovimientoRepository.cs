using ServiCuentas.Application.DTOs;
using ServiCuentas.Model;

namespace ServiCuentas.Infraestructure.Repository
{
    public interface IMovimientoRepository : IRepository<Movimiento>
    {
        Task<IEnumerable<Movimiento>> GetAllMovimientoFilterOrderPage(MovimientoFilterDTO filter, string order, int page, int pageSize);
    }
}
