using Microsoft.EntityFrameworkCore;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.SeedWork;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Infrastructure.Repositories
{
    public class TipoMaterialRepository
        : ITipoMaterialRepository
    {
        private readonly CatalogoMaterialesContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public TipoMaterialRepository(CatalogoMaterialesContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TipoMaterial Add(TipoMaterial tipoMaterial)
        {
            return _context.TipoMateriales.Add(tipoMaterial).Entity;
        }
        public TipoMaterial Update(TipoMaterial tipoMaterial)
        {
            return _context.TipoMateriales.Update(tipoMaterial).Entity;
        }

        public async Task<TipoMaterial> GetByIdAsync(Guid id)
        {
            var item = await _context
                                .TipoMateriales
                                .FirstOrDefaultAsync(o => o.Id == id);
            if (item == null)
            {
                item = _context
                            .TipoMateriales
                            .Local
                            .FirstOrDefault(o => o.Id == id);
            }

            return item;
        }

        public async Task<TipoMaterial> GetTipoMaterialesByNameAsync(string descripcion)
        {
            var tipo = await _context
                    .TipoMateriales
                    .FirstOrDefaultAsync(o => o.Descripcion == descripcion);
            if (tipo == null)
            {
                tipo = _context
                            .TipoMateriales
                            .Local
                            .FirstOrDefault(o => o.Descripcion == descripcion);
            }

            return tipo;
        }

    }
}