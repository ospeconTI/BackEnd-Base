using System;
using System.Collections.Generic;
using System.Linq;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.SeedWork;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Exceptions;


namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities
{
    public class Clasificacion : Entity, IAggregateRoot
    {

        public string Descripcion { get; set; }
        public List<Material> Materiales { get; }

        public Clasificacion()
        {

        }
        public Clasificacion(string descripcion) : this()
        {

            Descripcion = descripcion.Trim().ToUpper();

            this.AddDomainEvent(new ClasificacionAgregadoRequested(this));
        }
        public void Update(Guid id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion.Trim().ToUpper();
        }
    }
}