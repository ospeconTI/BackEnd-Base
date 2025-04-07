using System;
using System.Collections.Generic;
using System.Linq;
using OSPeConTI.SumariosIERIC.Domain.SeedWork;
using OSPeConTI.SumariosIERIC.Domain.Exceptions;
using OSPeConTI.SumariosIERIC.Domain.ValueObjects.Network;
using OSPeConTI.SumariosIERIC.Domain.Events;


namespace OSPeConTI.SumariosIERIC.Domain.Entities
{
    public class Empresa : Entity, IAggregateRoot
    {

        public string RazonSocial { get; private set; }
        public Cuit Cuit { get; private set; }
        public bool EsCooperativa { get; private set; }
        public bool EstadoActivo { get; private set; }

        private Empresa() { }
        public Empresa(Cuit cuit, string razonSocial, bool esCooperativa)
        {
            Cuit = cuit;
            RazonSocial = razonSocial;
            EsCooperativa = esCooperativa;
            EstadoActivo = false;
            this.AddDomainEvent(new EmpresaNuevaRequested(this));
        }

        public bool ActivarEmpresa()
        {
            EstadoActivo = true;
            return EstadoActivo;
        }

    }
}