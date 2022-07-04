using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.SeedWork;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities
{
    public interface IClasificacionRepository : IRepository<Clasificacion>
    {
        Clasificacion Add(Clasificacion clasificacion);
        Clasificacion Update(Clasificacion clasificacion);

        Task<Clasificacion> GetAsync(Guid Id);

        Task<Clasificacion> GetByNameAsync(string descrip);

    }
}