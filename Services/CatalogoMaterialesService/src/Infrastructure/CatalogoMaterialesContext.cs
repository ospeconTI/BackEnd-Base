using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;

using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.SeedWork;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Infrastructure.EntityConfigurations;
using OSPeConTI.BackEndBase.Services.CursosService.Domain.SeedWork;
using System.Linq;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Infrastructure
{
    public class CatalogoMaterialesContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "dbo";
        public DbSet<Clasificacion> Clasificaciones { get; set; }
        public DbSet<Material> Materiales { get; set; }
        public DbSet<TipoMaterial> TipoMateriales { get; set; }

        private readonly IMediator _mediator;
        private IDbContextTransaction _currentTransaction;

        public CatalogoMaterialesContext(DbContextOptions<CatalogoMaterialesContext> options) : base(options) { }

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;

        public CatalogoMaterialesContext(DbContextOptions<CatalogoMaterialesContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


            System.Diagnostics.Debug.WriteLine("CatalogoMaterialesContext::ctor ->" + this.GetHashCode());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClasificacionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MaterialesEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TipoMaterialEntityTypeConfiguration());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await SaveChangesAsync(cancellationToken);

            return true;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {

            this.ChangeTracker.DetectChanges();
            var added = this.ChangeTracker.Entries()
                        .Where(t => t.State == EntityState.Added)
                        .Select(t => t.Entity)
                        .ToArray();
            foreach (var entity in added)
            {
                if (entity is ITrack)
                {
                    var track = entity as ITrack;
                    track.Id = Guid.NewGuid();
                    track.FechaAlta = DateTime.Now;
                    track.UsuarioAlta = Thread.CurrentPrincipal != null ? Thread.CurrentPrincipal.Identity.Name : "Anonimo";
                    track.Activo = true;
                }
            }

            var modified = this.ChangeTracker.Entries()
                        .Where(t => t.State == EntityState.Modified)
                        .Select(t => t.Entity)
                        .ToArray();

            foreach (var entity in modified)
            {
                if (entity is ITrack)
                {
                    var track = entity as ITrack;
                    track.FechaUpdate = DateTime.Now;
                    track.UsuarioUpdate = Thread.CurrentPrincipal != null ? Thread.CurrentPrincipal.Identity.Name : "Anonimo";
                }
            }
            return await base.SaveChangesAsync(cancellationToken);

        }


        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }

    public class MaterialesContextDesignFactory : IDesignTimeDbContextFactory<CatalogoMaterialesContext>
    {

        public CatalogoMaterialesContext CreateDbContext(string[] args)
        {
            string env = args.Length == 0 ? "" : args[0];

            if (env != "Prod" && env != "Dev")
            {
                throw new Exception("Indique el entorno (\"Prod\" para produción o \"Dev\" para Desarrollo, ejemplo: dotnet ef database update -s ../application -- \"Prod\")");
            }

            if (env == "Prod")
            {
                var optionsBuilder = new DbContextOptionsBuilder<CatalogoMaterialesContext>()
                .UseSqlServer("Server=10.1.10.29;Initial Catalog=Materiales;User ID=sa;password=informacion");
                return new CatalogoMaterialesContext(optionsBuilder.Options, new NoMediator());
            }
            if (env == "Dev")
            {

                var optionsBuilder = new DbContextOptionsBuilder<CatalogoMaterialesContext>()
                .UseSqlServer("Server=sqldev01;Initial Catalog=CatalogoMateriales;integrated security=true");
                return new CatalogoMaterialesContext(optionsBuilder.Options, new NoMediator());
            }

            return null;

        }

        class NoMediator : IMediator
        {
            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
            {
                return Task.CompletedTask;
            }

            public Task Publish(object notification, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<TResponse>(default(TResponse));
            }

            public Task<object> Send(object request, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(default(object));
            }
        }
    }
}