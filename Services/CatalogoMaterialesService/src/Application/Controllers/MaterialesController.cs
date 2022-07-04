using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Commands;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Queries;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;


namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MaterialesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MaterialesController> _logger;

        private readonly IMaterialesQueries _materialesQueries;

        public MaterialesController(
            IMediator mediator,
            ILogger<MaterialesController> logger,
            IMaterialesQueries materiales)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _materialesQueries = materiales ?? throw new ArgumentNullException(nameof(materiales));
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> addMaterialessAsync([FromBody] AddMaterialesCommand command)
        {

            Guid UID = await _mediator.Send(command);

            return Ok(UID);
        }

        [Route("update")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> updateMaterialesAsync([FromBody] UpdateMaterialesCommand command)
        {
            bool commandResult = false;

            commandResult = await _mediator.Send(command);

            return Ok();
        }

        [Route("{codigo}")]
        [HttpGet]
        [ProducesResponseType(typeof(MaterialesDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult> GetMaterialesAsync(int codigo)
        {

            //Todo: It's good idea to take advantage of GetOrderByIdQuery and handle by GetCustomerByIdQueryHandler
            //var order customer = await _mediator.Send(new GetOrderByIdQuery(orderId));
            var materiales = await _materialesQueries.GetMaterialesAsync(codigo);

            return Ok(materiales);
        }

        [Route("all")]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            //Todo: It's good idea to take advantage of GetOrderByIdQuery and handle by GetCustomerByIdQueryHandler
            //var order customer = await _mediator.Send(new GetOrderByIdQuery(orderId));
            var materiales = await _materialesQueries.GetAll();

            return Ok(materiales);
        }


        [Route("getByName/{nombre}")]
        [HttpGet]
        public async Task<ActionResult> getByName(string nombre)
        {
            var materiales = await _materialesQueries.GetMaterialesByNameAsync(nombre);

            return Ok(materiales);
        }

        [Route("getByDescripcionesCombinadas/{descripcion}")]
        [HttpGet]
        public async Task<ActionResult> getByDescripcionesCombinadas(string descripcion)
        {
            var materiales = await _materialesQueries.GetMaterialeByDescripcionesCombinadasAsync(descripcion);

            return Ok(materiales);
        }

    }
}
