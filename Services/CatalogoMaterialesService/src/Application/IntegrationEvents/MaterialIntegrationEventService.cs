using System;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using OSPeConTI.BackEndBase.BuildingBlocks.EventBus.Abstractions;
using OSPeConTI.BackEndBase.BuildingBlocks.EventBus.Events;
using OSPeConTI.BackEndBase.BuildingBlocks.IntegrationEventLogEF;
using OSPeConTI.BackEndBase.BuildingBlocks.IntegrationEventLogEF.Services;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Infrastructure;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.IntegrationEvents
{
    public class MaterialIntegrationEventService : IMaterialIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly CatalogoMaterialesContext _catalogoMaterialesContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<MaterialIntegrationEventService> _logger;

        public MaterialIntegrationEventService(IEventBus eventBus,
            CatalogoMaterialesContext catalogoMaterialesContext,
            IntegrationEventLogContext eventLogContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
            ILogger<MaterialIntegrationEventService> logger)
        {
            _catalogoMaterialesContext = catalogoMaterialesContext ?? throw new ArgumentNullException(nameof(catalogoMaterialesContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_catalogoMaterialesContext.Database.GetDbConnection());
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
        {
            var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

            foreach (var logEvt in pendingLogEvents)
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", logEvt.EventId, Program.AppName, logEvt.IntegrationEvent);

                try
                {
                    await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                    _eventBus.Publish(logEvt.IntegrationEvent);
                    await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from {AppName}", logEvt.EventId, Program.AppName);

                    await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
                }
            }
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent evt, Guid transactionId)
        {
            _logger.LogInformation("----- Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

            //IDbContextTransaction transactionId = _catalogoMaterialesContext.GetCurrentTransaction();

            await _eventLogService.SaveEventAsync(evt, transactionId);

        }


    }
}