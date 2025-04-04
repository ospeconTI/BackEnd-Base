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
    public class ActivarEmpresaCommand : IRequest<Guid>
    {

        [DataMember]
        public Guid EmpresaId { get; set; }


        public ActivarEmpresaCommand() { }
        public ActivarEmpresaCommand(Guid empresaId)

        {
            EmpresaId = empresaId;
        }
    }
}