using MediatR;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Commands
{
    [DataContract]
    public class AddTipoMaterialCommand : IRequest<bool>
    {
        [DataMember]
        public string Descripcion { get; set; }

        public AddTipoMaterialCommand()
        {

        }
        public AddTipoMaterialCommand(string descripcion)

        {
            Descripcion = descripcion;
        }

    }
}