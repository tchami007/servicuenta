using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ServiCuentas.Data;
using ServiCuentas.Shared;

namespace ServiCuentas.Infraestructure.Repository
{
    public interface IUnitOfWork
    {
        ICuentaRepository _cuentaRepository { get; }
        IMovimientoRepository _movimientoRepository { get; }
        INumeracionRepository _numeracionRepository { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation);
        Task ExecuteAsync(Func<Task> operation);

    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;
        public ICuentaRepository _cuentaRepository { get; }
        public IMovimientoRepository _movimientoRepository { get; }
        public INumeracionRepository _numeracionRepository { get; }

        private IDbContextTransaction _transaction;

        public UnitOfWork(AppDBContext context, ICuentaRepository cuentaRepository, IMovimientoRepository movimientoRepository, INumeracionRepository numeracionRepository)
        {
            _context = context;
            _cuentaRepository = cuentaRepository;
            _movimientoRepository = movimientoRepository;
            _numeracionRepository = numeracionRepository;
        }
        public async Task BeginTransactionAsync()
        {
            
            var transac = _context.Database.CurrentTransaction; // Busca una transaccion pre-existente

            if (transac != null) // si exite...
            {
                _transaction = transac; // la asigna
            }

            if (_transaction == null ) // si no existe transaccion...
            {
                _transaction = await _context.Database.BeginTransactionAsync(); // comienza na transaccion
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                await _transaction.RollbackAsync();
            }
        }


        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation) 
        { var strategy = _context.Database.CreateExecutionStrategy(); 
            return await strategy.ExecuteAsync(async () => 
            { using (var transaction = await _context.Database.BeginTransactionAsync()) 
                { var result = await operation(); 
                    await _context.SaveChangesAsync(); 
                    await transaction.CommitAsync(); 
                    return result; 
                } 
            }); 
        }
        public async Task ExecuteAsync(Func<Task> operation)
        {
            var strategy = _context.Database.CreateExecutionStrategy(); 
            await strategy.ExecuteAsync(async () => 
            { using (var transaction = await _context.Database.BeginTransactionAsync()) 
                { await operation(); 
                    await _context.SaveChangesAsync(); 
                    await transaction.CommitAsync(); 
                } 
            });
        }
    }
}