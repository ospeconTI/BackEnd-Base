using MediatR;
namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities
{

    public class ClasificacionAgregadoRequested : INotification
    {
        public Clasificacion Clasificacion { get; set; }
        public ClasificacionAgregadoRequested(Clasificacion clasificacion)
        {
            Clasificacion = clasificacion;
        }
    }

}
