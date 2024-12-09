using ServiCuentas.Model;

namespace ServiCuentas.Infraestructure.Repository
{
    public interface IFechaProcesoRepository : IRepository<FechaProceso>
    {
        Task<bool> EsFechaProceso(DateTime fecha, string descripcion);
        Task<DateTime> GetFecha(string descripcion);
    }
}
