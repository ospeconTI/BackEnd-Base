using MediatR;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Commands
{
    // Regular CommandHandler
    public class UpdateClasificacionCommandHandler : IRequestHandler<UpdateClasificacionCommand, bool>
    {
        private readonly IClasificacionRepository _clasificacionRepository;

        public UpdateClasificacionCommandHandler(IClasificacionRepository clasificacionRepository)
        {
            _clasificacionRepository = clasificacionRepository;
        }

        public async Task<bool> Handle(UpdateClasificacionCommand command, CancellationToken cancellationToken)
        {

            var clasificacionToUpdate = await _clasificacionRepository.GetAsync(command.Id);

            if (clasificacionToUpdate == null)
            {
                return false;
            }

            clasificacionToUpdate.Update(command.Id, command.Descripcion);

            _clasificacionRepository.Update(clasificacionToUpdate);

            return await _clasificacionRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}