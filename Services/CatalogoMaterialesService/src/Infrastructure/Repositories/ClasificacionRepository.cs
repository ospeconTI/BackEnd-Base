using Microsoft.EntityFrameworkCore;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.SeedWork;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Infrastructure.Repositories
{
    public class ClasificacionRepository
        : IClasificacionRepository
    {
        private readonly CatalogoMaterialesContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public ClasificacionRepository(CatalogoMaterialesContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Clasificacion Add(Clasificacion clasificacion)
        {
            return _context.Clasificaciones.Add(clasificacion).Entity;
        }
        public Clasificacion Update(Clasificacion clasificacion)
        {
            return _context.Clasificaciones.Update(clasificacion).Entity;
        }

        public async Task<Clasificacion> GetAsync(Guid id)
        {
            var item = await _context
                                .Clasificaciones
                                .FirstOrDefaultAsync(o => o.Id == id);
            if (item == null)
            {
                item = _context
                            .Clasificaciones
                            .Local
                            .FirstOrDefault(o => o.Id == id);
            }

            return item;
        }

        public async Task<Clasificacion> GetByNameAsync(string descripcion)
        {
            var clasif = await _context
                    .Clasificaciones
                    .FirstOrDefaultAsync(o => o.Descripcion == descripcion);

            return clasif;
        }

    }
}