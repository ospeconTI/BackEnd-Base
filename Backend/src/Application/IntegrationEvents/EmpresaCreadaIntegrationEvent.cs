using System;
using System.Text.Json.Serialization;
using OSPeConTI.SumariosIERIC.BuildingBlocks.EventBus.Events;

namespace OSPeConTI.SumariosIERIC.Application.IntegrationEvents
{
    public record EmpresaCreadaIntegrationEvent : IntegrationEvent
    {
        [JsonInclude]
        public Guid MaterialId { get; set; }

        [JsonConstructor]
        public EmpresaCreadaIntegrationEvent(Guid materialId)
        {
            MaterialId = materialId;

        }
    }
}