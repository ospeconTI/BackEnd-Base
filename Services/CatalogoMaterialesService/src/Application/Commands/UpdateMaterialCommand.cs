using MediatR;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Commands
{
    [DataContract]
    public class UpdateMaterialesCommand : IRequest<bool>
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public int Codigo { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public decimal Costo { get; set; }
        [DataMember]
        public Guid ClasificacionId { get; set; }
        [DataMember]
        public Guid TipoMaterialId { get; set; }

        public UpdateMaterialesCommand()
        {

        }

        public UpdateMaterialesCommand(Guid id, int codigo, string descripcion, decimal costo, Guid clasificacionId, Guid idTipoMaterial)
        {
            Id = id;
            Codigo = codigo;
            Descripcion = descripcion;
            Costo = costo;
            ClasificacionId = clasificacionId;
            TipoMaterialId = idTipoMaterial;
        }
    }
}