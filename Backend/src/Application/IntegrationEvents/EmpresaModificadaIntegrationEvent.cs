using System;
using System.Text.Json.Serialization;
using OSPeConTI.SumariosIERIC.BuildingBlocks.EventBus.Events;

namespace OSPeConTI.SumariosIERIC.Application.IntegrationEvents
{
    public record EmpresaModificadaIntegrationEvent : IntegrationEvent
    {
        [JsonInclude]
        public Guid EmpresaId { get; set; }
        [JsonConstructor]
        public EmpresaModificadaIntegrationEvent(Guid empresaId)
        {
            EmpresaId = empresaId;

        }
    }
}