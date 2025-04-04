using MediatR;
using OSPeConTI.SumariosIERIC.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System;
using OSPeConTI.SumariosIERIC.Domain.Enums;
using OSPeConTI.SumariosIERIC.Domain.ValueObjects.Network;
using OSPeConTI.SumariosIERIC.Domain.Exceptions;

namespace OSPeConTI.SumariosIERIC.Application.Commands
{
    // Regular CommandHandler
    public class ActivarEmpresaCommandHandler : IRequestHandler<ActivarEmpresaCommand, Guid>
    {
        private readonly IEmpresaRepository _EmpresasRepository;

        public ActivarEmpresaCommandHandler(IEmpresaRepository EmpresaRepository)
        {
            _EmpresasRepository = EmpresaRepository;
        }

        public async Task<Guid> Handle(ActivarEmpresaCommand command, CancellationToken cancellationToken)
        {
            Empresa empresa = await _EmpresasRepository.GetById(command.EmpresaId);
            if (empresa == null) throw new SumariosDomainException("No se encontró la empresa");
            _EmpresasRepository.ActivarEmpresa(empresa);
            await _EmpresasRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return empresa.Id;
        }
    }
}