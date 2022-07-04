using System;
using System.Text.Json.Serialization;
using OSPeConTI.BackEndBase.BuildingBlocks.EventBus.Events;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.IntegrationEvents
{
    public record MaterialModificadoIntegrationEvent : IntegrationEvent
    {
        [JsonInclude]
        public Guid MaterialId { get; set; }
        [JsonConstructor]
        public MaterialModificadoIntegrationEvent(Guid materialId)
        {
            MaterialId = materialId;

        }
    }
}