using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Infrastructure.EntityConfigurations
{
    class ClasificacionEntityTypeConfiguration : IEntityTypeConfiguration<Clasificacion>
    {
        public void Configure(EntityTypeBuilder<Clasificacion> ClasificacionConfiguration)
        {
            ClasificacionConfiguration.ToTable("Clasificaciones", CatalogoMaterialesContext.DEFAULT_SCHEMA);

            ClasificacionConfiguration.HasKey(o => o.Id);

            ClasificacionConfiguration.Ignore(b => b.DomainEvents);

        }
    }
}