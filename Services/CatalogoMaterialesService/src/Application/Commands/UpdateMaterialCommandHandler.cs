using MediatR;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Exceptions;
using OSPeConTI.BackEndBase.BuildingBlocks.EventBus.Events;
using OSPeConTI.BackEndBase.BuildingBlocks.EventBus.Abstractions;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.IntegrationEvents;
using OSPeConTI.BackEndBase.BuildingBlocks.IntegrationEventLogEF.Services;
using System.Data.Common;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Commands
{
    // Regular CommandHandler
    public class UpdateMaterialesCommandHandler : IRequestHandler<UpdateMaterialesCommand, bool>
    {
        private readonly IMaterialesRepository _materialesRepository;

        private readonly CatalogoMaterialesContext _catalogoMaterialesContext;

        private readonly IMaterialIntegrationEventService _materialIntegrationEventService;

        public UpdateMaterialesCommandHandler(IMaterialesRepository materialesRepository, CatalogoMaterialesContext catalogoMaterialesContext, IMaterialIntegrationEventService materialIntegrationEventService)
        {
            _materialesRepository = materialesRepository;

            _catalogoMaterialesContext = catalogoMaterialesContext;

            _materialIntegrationEventService = materialIntegrationEventService;

        }

        public async Task<bool> Handle(UpdateMaterialesCommand command, CancellationToken cancellationToken)
        {

            var materialesToUpdate = await _materialesRepository.GetByCodigoAsync(command.Codigo);

            if (materialesToUpdate == null) throw new NotFoundException();

            materialesToUpdate.Update(command.Id, command.Codigo, command.Descripcion, command.Costo, command.ClasificacionId, command.TipoMaterialId);

            _materialesRepository.Update(materialesToUpdate);

            await _materialesRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            MaterialModificadoIntegrationEvent evento = new MaterialModificadoIntegrationEvent(command.Id);


            Guid transactionId = Guid.NewGuid();
            await _materialIntegrationEventService.AddAndSaveEventAsync(evento, transactionId);
            await _materialIntegrationEventService.PublishEventsThroughEventBusAsync(transactionId);


            return true;
        }
    }
}