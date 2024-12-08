using ServiCuentas.Shared;

namespace ServiCuentas.Infraestructure.Repository
{
    public interface IRepository<T>
    {
        Task<Result<T>> AddItem(T item);
        Task<Result<T>> UpdateItem(T item);
        Task<Result<T>> DeleteItem(int id);
        Task<Result<T>> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllOrderPage(string order, int page, int pageSize);
    }
}
