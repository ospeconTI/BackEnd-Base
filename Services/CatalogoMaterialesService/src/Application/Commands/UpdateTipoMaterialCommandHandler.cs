using MediatR;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Exceptions;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Commands
{
    // Regular CommandHandler
    public class UpdateTipoMaterialCommandHandler : IRequestHandler<UpdateTipoMaterialCommand, bool>
    {
        private readonly ITipoMaterialRepository _tipoMaterialRepository;

        public UpdateTipoMaterialCommandHandler(ITipoMaterialRepository tipoMaterialRepository)
        {
            _tipoMaterialRepository = tipoMaterialRepository;
        }

        public async Task<bool> Handle(UpdateTipoMaterialCommand command, CancellationToken cancellationToken)
        {

            var tipoMaterialToUpdate = await _tipoMaterialRepository.GetByIdAsync(command.Id);

            if (tipoMaterialToUpdate == null) throw new NotFoundException();

            tipoMaterialToUpdate.Update(command.Id, command.Descripcion);

            _tipoMaterialRepository.Update(tipoMaterialToUpdate);

            return await _tipoMaterialRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}