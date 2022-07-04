using MediatR;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System;
using OSPeConTI.BackEndBase.BuildingBlocks.EventBus.Abstractions;
using OSPeConTI.BackEndBase.BuildingBlocks.EventBus.Events;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.IntegrationEvents;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Commands
{
    // Regular CommandHandler
    public class AddMaterialesCommandHandler : IRequestHandler<AddMaterialesCommand, Guid>
    {
        private readonly IMaterialesRepository _materialesRepository;
        private readonly IEventBus _eventBus;

        public AddMaterialesCommandHandler(IMaterialesRepository materialesRepository, IEventBus eventBus)
        {
            _materialesRepository = materialesRepository;
            _eventBus = eventBus;
        }

        public async Task<Guid> Handle(AddMaterialesCommand command, CancellationToken cancellationToken)
        {

            Material material = new Material(command.Codigo, command.Descripcion, command.Costo, command.ClasificacionId, command.TipoMaterialId);

            _materialesRepository.Add(material);

            await _materialesRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            MaterialCreadoIntegrationEvent evento = new MaterialCreadoIntegrationEvent(material.Id);

            _eventBus.Publish(evento);
            return material.Id;
        }
    }
}