using System.Threading;
using System.Threading.Tasks;
using MediatR;
namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities
{

    public class ClasificacionAgregadoHandler : INotificationHandler<ClasificacionAgregadoRequested>
    {
        private readonly IClasificacionRepository _clasificacionRepository;
        public ClasificacionAgregadoHandler(IClasificacionRepository clasificacionRepository)
        {
            _clasificacionRepository = clasificacionRepository;
        }
        public Task Handle(ClasificacionAgregadoRequested notificacion, CancellationToken cancellationToken)
        {
            //Valido que el codigo tenga un valor
            string descripcion = notificacion.Clasificacion.Descripcion.Trim().ToUpper();
            Clasificacion clasificacion;

            //Valido que la descripcion tenga algun valor
            if (descripcion == "") throw new System.InvalidOperationException("La Descripción no puede estar vacía");

            //Busco si la descripcion ya existe
            clasificacion = _clasificacionRepository.GetByNameAsync(descripcion).GetAwaiter().GetResult();
            if (clasificacion != null) throw new System.InvalidOperationException("Ya existe una Clasificacion con la descripción '" + descripcion + "'");

            return Task.CompletedTask;
        }
    }

}
