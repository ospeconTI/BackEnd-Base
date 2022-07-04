using System;
using System.Collections.Generic;
using System.Linq;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.SeedWork;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Exceptions;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities
{
    public class Material : Entity, IAggregateRoot
    {

        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Costo { get; set; }
        public Guid? ClasificacionId { get; set; }
        public Guid? TipoMaterialId { get; set; }

        public Clasificacion Clasificacion { get; set; }
        public TipoMaterial TipoMaterial { get; set; }

        public Material()
        {
        }
        public Material(int codigo, string descripcion, decimal costo, Guid? idClasif, Guid? idTipoMaterial) : this()
        {
            Codigo = codigo;
            Descripcion = descripcion.Trim().ToUpper();
            Costo = costo;
            ClasificacionId = idClasif;
            TipoMaterialId = idTipoMaterial;
            this.AddDomainEvent(new MaterialAgregadoRequested(this));
        }
        public void Update(Guid id, int codigo, string descripcion, decimal costo, Guid? idClasif, Guid? idTipoMaterial)
        {
            Id = id;
            Codigo = codigo;
            Descripcion = descripcion.Trim().ToUpper();
            Costo = costo;
            ClasificacionId = idClasif;
            TipoMaterialId = idTipoMaterial;
        }
    }
}