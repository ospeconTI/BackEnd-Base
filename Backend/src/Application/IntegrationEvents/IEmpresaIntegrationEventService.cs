using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using OSPeConTI.SumariosIERIC.BuildingBlocks.EventBus.Events;

namespace OSPeConTI.SumariosIERIC.Application.IntegrationEvents
{

    public interface IEmpresaIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt, Guid transacationId);
    }
}