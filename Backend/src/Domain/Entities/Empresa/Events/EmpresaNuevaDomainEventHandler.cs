using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OSPeConTI.SumariosIERIC.Domain.Entities;
using OSPeConTI.SumariosIERIC.Domain.Enums;
using OSPeConTI.SumariosIERIC.Domain.Exceptions;
using OSPeConTI.SumariosIERIC.Domain.ValueObjects.Network;
namespace OSPeConTI.SumariosIERIC.Domain.Events
{
    public class EmpresaNuevaDomainEventHandler : INotificationHandler<EmpresaNuevaRequested>
    {
        private IEmpresaRepository EmpresaRepository;
        public EmpresaNuevaDomainEventHandler(IEmpresaRepository empresaRepository)
        {
            EmpresaRepository = empresaRepository;
        }

        public async Task Handle(EmpresaNuevaRequested notification, CancellationToken cancellationToken)
        {
            if (await EmpresaRepository.ExistAny(notification.Empresa.Cuit)) throw new SumariosDomainException("La empresa ya se encuentra cargada en el sistema");
        }
    }
}