using MediatR;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Commands
{
    // Regular CommandHandler
    public class AddClasificacionCommandHandler : IRequestHandler<AddClasificacionCommand, Guid>
    {
        private readonly IClasificacionRepository _clasificacionRepository;

        public AddClasificacionCommandHandler(IClasificacionRepository clasificacionRepository)
        {
            _clasificacionRepository = clasificacionRepository;
        }

        public async Task<Guid> Handle(AddClasificacionCommand command, CancellationToken cancellationToken)
        {

            Clasificacion clasificacion = new Clasificacion(command.Descripcion);

            _clasificacionRepository.Add(clasificacion);

            await _clasificacionRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return clasificacion.Id;
        }
    }
}