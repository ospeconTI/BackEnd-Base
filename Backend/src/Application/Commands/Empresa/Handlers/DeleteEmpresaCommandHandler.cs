using MediatR;
using OSPeConTI.SumariosIERIC.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System;
using OSPeConTI.SumariosIERIC.Infrastructure.Repositories;
using OSPeConTI.SumariosIERIC.Domain.Exceptions;

namespace OSPeConTI.SumariosIERIC.Application.Commands
{
    // Regular CommandHandler
    public class DeleteEmpresaCommandHandler : IRequestHandler<DeleteEmpresaCommand, Guid>
    {
        private readonly IEmpresaRepository _EmpresaRepository;

        public DeleteEmpresaCommandHandler(IEmpresaRepository EmpresaRepository)
        {
            _EmpresaRepository = EmpresaRepository;
        }

        public async Task<Guid> Handle(DeleteEmpresaCommand command, CancellationToken cancellationToken)
        {
            Empresa Empresa = await _EmpresaRepository.GetById(command.EmpresaId);
            if (Empresa == null) throw new SumariosDomainException("No se encontró la Empresa");
            _EmpresaRepository.Delete(Empresa);
            await _EmpresaRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Empresa.Id;
        }
    }
}

