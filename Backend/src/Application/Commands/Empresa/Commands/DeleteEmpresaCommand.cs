using MediatR;
using OSPeConTI.SumariosIERIC.Domain.Entities;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

namespace OSPeConTI.SumariosIERIC.Application.Commands
{
    [DataContract]
    public class DeleteEmpresaCommand : IRequest<Guid>
    {
        [DataMember]
        public Guid EmpresaId { get; set; }

        public DeleteEmpresaCommand() { }
        public DeleteEmpresaCommand(Guid empresaId)
        {
            EmpresaId = empresaId;
        }
    }
}