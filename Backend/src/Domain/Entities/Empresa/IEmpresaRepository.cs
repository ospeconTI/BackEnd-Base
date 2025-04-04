using OSPeConTI.SumariosIERIC.Domain.SeedWork;
using OSPeConTI.SumariosIERIC.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using OSPeConTI.SumariosIERIC.Domain.ValueObjects.Network;

namespace OSPeConTI.SumariosIERIC.Domain.Entities
{
    public interface IEmpresaRepository : IRepository<Empresa>
    {
        Empresa Add(Empresa Empresa);
        Task<Empresa> GetById(Guid id);
        Task<Empresa> GetByCuit(Int64 cuil);
        Empresa Delete(Empresa Empresa);
        bool ActivarEmpresa(Empresa Empresa);
        Task<bool> ExistAny(Cuit cuit);

    }
}