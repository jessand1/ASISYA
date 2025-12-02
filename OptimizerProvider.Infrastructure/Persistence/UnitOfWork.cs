using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProviderOptimizer.Application.Interfaces;
using ProviderOptimizer.Domain.Interfaces;
using ProviderOptimizer.Infrastructure.Persistence.Repositories;
using ProviderOptimizer.Persistence;
using System.Collections;

namespace ProviderOptimizer.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProviderOptimizerDbContext _context;
    private IDbContextTransaction? _transaction;

    // Contenedor de repositorios creados dinámicamente
    private readonly Hashtable _repositories = new();

    public UnitOfWork(ProviderOptimizerDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<T> Repository<T>() where T : class
    {
        var typeName = typeof(T).Name;

        // Si el repositorio no existe en caché -> lo crea
        if (!_repositories.ContainsKey(typeName))
        {
            var repositoryInstance = new GenericRepository<T>(_context);
            _repositories[typeName] = repositoryInstance;
        }

        return (IGenericRepository<T>)_repositories[typeName]!;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction ??= await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();

        if (_transaction != null)
            await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
            await _transaction.RollbackAsync();
    }

    public Task<int> SaveAsync()
        => _context.SaveChangesAsync();

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }

    public async Task TestConnectionAsync()
    {
        await _context.Database.ExecuteSqlRawAsync("SELECT 1;");
    }
}
