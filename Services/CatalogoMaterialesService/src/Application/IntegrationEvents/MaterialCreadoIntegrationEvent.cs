using System;
using System.Text.Json.Serialization;
using OSPeConTI.BackEndBase.BuildingBlocks.EventBus.Events;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.IntegrationEvents
{
    public record MaterialCreadoIntegrationEvent : IntegrationEvent
    {
        [JsonInclude]
        public Guid MaterialId { get; set; }

        [JsonConstructor]
        public MaterialCreadoIntegrationEvent(Guid materialId)
        {
            MaterialId = materialId;

        }
    }
}