using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSPeConTI.SumariosIERIC.Application.Commands;
using OSPeConTI.SumariosIERIC.Application.Queries;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OSPeConTI.SumariosIERIC.Application
{
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize]
    public class EmpresaController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmpresaController> _logger;
        private readonly IEmpresaQueries _EmpresaQueries;
        public EmpresaController(
            IMediator mediator,
            ILogger<EmpresaController> logger,
            IEmpresaQueries Empresas
       )
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _EmpresaQueries = Empresas ?? throw new ArgumentNullException(nameof(Empresas));
        }

        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> AddEmpresaAsync([FromBody] AddEmpresaCommand command)
        {
            Guid id = await _mediator.Send(command);
            return Ok(id);
        }

        [Route("GetById/{id}")]
        [HttpGet]
        public async Task<ActionResult> getById(Guid id)
        {
            try
            {
                var Empresa = await _EmpresaQueries.GetById(id);
                return Ok(Empresa);
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("GetByCuit/{cuit}")]
        [HttpGet]
        public async Task<ActionResult> GetByCuit(Int64 cuit)
        {
            try
            {
                var Empresa = await _EmpresaQueries.GetByCuit(cuit);
                return Ok(Empresa);
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("All")]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var retorno = await _EmpresaQueries.GetAll();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

        [Route("Quitar/{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> Quitar(Guid id)

        {
            Guid retorno = await _mediator.Send(new DeleteEmpresaCommand(id));
            return Ok(retorno);
        }

        [Route("ActivarEmpresa")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> ActivarEmpresaAsync([FromBody] ActivarEmpresaCommand command)
        {
            var commandResult = await _mediator.Send(command);
            return Ok(commandResult);
        }
    }
}