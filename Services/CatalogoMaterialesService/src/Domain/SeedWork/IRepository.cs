namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.SeedWork
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}