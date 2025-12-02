
using ProviderOptimizer.Domain.Interfaces;

namespace ProviderOptimizer.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task TestConnectionAsync();
    IGenericRepository<T> Repository<T>() where T : class;

    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();

    Task<int> SaveAsync();
}
