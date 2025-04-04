using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using OSPeConTI.SumariosIERIC.Application.Exceptions;
using OSPeConTI.SumariosIERIC.Domain.Entities;
using OSPeConTI.SumariosIERIC.Domain.Enums;
using OSPeConTI.SumariosIERIC.Domain.ValueObjects.Network;

namespace OSPeConTI.SumariosIERIC.Application.Queries
{
    public class EmpresaDTO
    {
        public EmpresaDTO(Empresa Empresa)
        {
            if (Empresa == null) throw new NotFoundException();
            Id = Empresa.Id;
            CUIT = Empresa.Cuit;
            RazonSocial = Empresa.RazonSocial;
            EsCooperativa = Empresa.EsCooperativa;
            EstadoActivo = Empresa.EstadoActivo;
        }
        public Guid Id { get; set; }
        public Int64 CUIT { get; set; }
        public string RazonSocial { get; set; }
        public bool EsCooperativa { get; set; }
        public bool EstadoActivo { get; set; }
    }
}