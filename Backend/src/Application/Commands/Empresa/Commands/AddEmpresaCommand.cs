using MediatR;
using OSPeConTI.SumariosIERIC.Domain.Entities;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using OSPeConTI.SumariosIERIC.Domain.Enums;
using OSPeConTI.SumariosIERIC.Domain.ValueObjects.Network;

namespace OSPeConTI.SumariosIERIC.Application.Commands
{
    [DataContract]
    public class AddEmpresaCommand : IRequest<Guid>
    {

        [DataMember]
        public Int64 CUIT { get; set; }
        [DataMember]
        public string RazonSocial { get; set; }
        [DataMember]
        public bool EsCooperativa { get; set; }

        public AddEmpresaCommand() { }
        public AddEmpresaCommand(Int64 cuit, string razonSocial, bool esCooperativa)

        {
            CUIT = cuit;
            RazonSocial = razonSocial;
            EsCooperativa = esCooperativa;
        }
    }
}