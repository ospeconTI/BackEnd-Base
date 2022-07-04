using System.Threading;
using System.Threading.Tasks;
using MediatR;
namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities
{

    public class MaterialAgregadoHandler : INotificationHandler<MaterialAgregadoRequested>
    {
        private readonly IMaterialesRepository _materialesRepository;
        public MaterialAgregadoHandler(IMaterialesRepository materialesRepository)
        {
            _materialesRepository = materialesRepository;
        }
        public Task Handle(MaterialAgregadoRequested notificacion, CancellationToken cancellationToken)
        {
            //Valido que el codigo tenga un valor
            int codigo = notificacion.Material.Codigo;
            string descripcion = notificacion.Material.Descripcion.Trim().ToUpper();
            Material material;

            if (codigo == 0) throw new System.InvalidOperationException("Debe ingresar un codigo");

            //Busco que el codigo no exista
            //material = _materialesRepository.ValidoCodigoExistenteAsync(codigo).GetAwaiter().GetResult();
            material = _materialesRepository.ValidoCodigoExistenteAsync(codigo);
            if (material != null) throw new System.InvalidOperationException("Ya existe un material con el código " + codigo.ToString());

            //Valido que la descripcion tenga algun valor
            if (descripcion == "") throw new System.InvalidOperationException("La Descripción debe tener un valor");

            //Busco si la descripcion ya existe
            material = _materialesRepository.ValidoDescripcionExistenteAsync(descripcion).GetAwaiter().GetResult();
            if (material != null) throw new System.InvalidOperationException("Ya existe un material con la descripción '" + descripcion + "'");

            return Task.CompletedTask;
        }
    }

}
