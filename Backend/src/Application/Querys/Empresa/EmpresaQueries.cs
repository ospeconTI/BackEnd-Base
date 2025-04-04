namespace OSPeConTI.SumariosIERIC.Application.Queries
{
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using System.Linq;
    using OSPeConTI.SumariosIERIC.Infrastructure;
    using OSPeConTI.SumariosIERIC.Domain.Entities;
    using OSPeConTI.SumariosIERIC.Domain.ValueObjects.Network;
    using Microsoft.EntityFrameworkCore;
    using Auth;

    public class EmpresaQueries : IEmpresaQueries
    {
        public SumariosContext _SumariosContext { get; set; }
        public EmpresaQueries(SumariosContext SumariosContext)
        {
            _SumariosContext = SumariosContext;
        }
        public async Task<IEnumerable<EmpresaDTO>> GetAll()
        {
            return _SumariosContext.Empresas


            .Where(empresa => empresa.Activo == true).OrderBy(empresa => empresa.FechaAlta).Select(Empresa => new EmpresaDTO(Empresa));
        }
        public async Task<EmpresaDTO> GetById(Guid id)
        {
            return new EmpresaDTO(_SumariosContext.Empresas

            .FirstOrDefault(t => t.Id == id));
        }

        public async Task<EmpresaDTO> GetByCuit(Int64 cuit)
        {
            return new EmpresaDTO(_SumariosContext.Empresas
            .FirstOrDefault(e => e.Cuit == cuit));
        }


    }
}