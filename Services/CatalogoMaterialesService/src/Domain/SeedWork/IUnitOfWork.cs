using System;
using System.Threading;
using System.Threading.Tasks;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}