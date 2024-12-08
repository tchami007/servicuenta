using ServiCuentas.Model;
using ServiCuentas.Shared;

namespace ServiCuentas.Infraestructure.Repository
{
    public interface INumeracionRepository : IRepository<Numeracion>
    {
        Task<Result<Numeracion>> GetByDescripcion(string descripcion);
    }
}
