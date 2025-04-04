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
    public class AddEmpresaCommandHandler : IRequestHandler<AddEmpresaCommand, Guid>
    {
        private readonly IEmpresaRepository _EmpresasRepository;

        public AddEmpresaCommandHandler(IEmpresaRepository EmpresaRepository)
        {
            _EmpresasRepository = EmpresaRepository;
        }

        public async Task<Guid> Handle(AddEmpresaCommand command, CancellationToken cancellationToken)
        {
            Empresa empresaNueva = new Empresa((Cuit)command.CUIT, command.RazonSocial, command.EsCooperativa);
            _EmpresasRepository.Add(empresaNueva);
            await _EmpresasRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return empresaNueva.Id;
        }
    }
}