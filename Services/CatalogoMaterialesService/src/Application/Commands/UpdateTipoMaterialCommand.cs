using MediatR;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Commands
{
    [DataContract]
    public class UpdateTipoMaterialCommand : IRequest<bool>
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Descripcion { get; set; }

        public UpdateTipoMaterialCommand()
        {

        }

        public UpdateTipoMaterialCommand(Guid id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;
        }
    }
}