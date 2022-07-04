using Microsoft.EntityFrameworkCore;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.SeedWork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Infrastructure.Repositories
{
    public class MaterialesRepository
        : IMaterialesRepository
    {
        private readonly CatalogoMaterialesContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public MaterialesRepository(CatalogoMaterialesContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Material> GetByCodigoAsync(int codigo)
        {
            var item = await _context
                                .Materiales
                                .FirstOrDefaultAsync(o => o.Codigo == codigo);

            //if (item == null) //item = new Material();

            // {
            //     item = _context
            //                 .Materiales
            //                 .Local
            //                 .FirstOrDefault(o => o.Codigo == codigo);
            // }

            return item;
        }

        public async Task<Material> GetMaterialesByNameAsync(string descripcion)
        {
            var material = await _context
                                .Materiales
                                .FirstOrDefaultAsync(o => o.Descripcion == descripcion);

            // if (material == null)
            // {
            //     material = _context
            //                 .Materiales
            //                 .Local
            //                 .FirstOrDefault(o => o.Descripcion == descripcion);
            // }

            return material;
        }


        public Material ValidoCodigoExistenteAsync(int codigo)
        {
            var item = _context
                                .Materiales
                                .Where(o => o.Codigo == codigo);

            return item.FirstOrDefault<Material>();
        }

        public async Task<Material> ValidoDescripcionExistenteAsync(string descripcion)
        {
            var material = await _context
                                .Materiales
                                .FirstOrDefaultAsync(o => o.Descripcion == descripcion);

            return material;
        }

        public Material Add(Material material)
        {
            return _context.Materiales.Add(material).Entity;
        }

        public Material Update(Material material)
        {
            return _context.Materiales.Update(material).Entity;
        }
    }
}