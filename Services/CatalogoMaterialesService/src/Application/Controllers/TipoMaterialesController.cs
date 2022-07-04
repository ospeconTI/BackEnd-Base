using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Commands;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Queries;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TipoMaterialesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TipoMaterialesController> _logger;

        private readonly ITipoMaterialesQueries _tipoMaterialesQueries;

        public TipoMaterialesController(
            IMediator mediator,
            ILogger<TipoMaterialesController> logger,
            ITipoMaterialesQueries tipoMateriales)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tipoMaterialesQueries = tipoMateriales ?? throw new ArgumentNullException(nameof(tipoMateriales));
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetTipoMaterialesAsync(Guid id)
        {
            try
            {
                //Todo: It's good idea to take advantage of GetOrderByIdQuery and handle by GetCustomerByIdQueryHandler
                //var order customer = await _mediator.Send(new GetOrderByIdQuery(orderId));
                var sector = await _tipoMaterialesQueries.GetTipoMaterialesAsync(id);

                return Ok(sector);
            }
            catch
            {
                return NotFound();
            }
        }


        [Route("getByName/{descripcion}")]
        [HttpGet]
        public async Task<ActionResult> GetTipoMaterialesByNameAsync(string descripcion)
        {
            try
            {
                //Todo: It's good idea to take advantage of GetOrderByIdQuery and handle by GetCustomerByIdQueryHandler
                //var order customer = await _mediator.Send(new GetOrderByIdQuery(orderId));
                var legajo = await _tipoMaterialesQueries.GetTipoMaterialesByNameAsync(descripcion);

                return Ok(legajo);
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("all")]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                //Todo: It's good idea to take advantage of GetOrderByIdQuery and handle by GetCustomerByIdQueryHandler
                //var order customer = await _mediator.Send(new GetOrderByIdQuery(orderId));
                var tipoMateriales = await _tipoMaterialesQueries.GetAll();

                return Ok(tipoMateriales);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> addTipoMaterialesAsync([FromBody] AddTipoMaterialCommand command)
        {


            await _mediator.Send(command);


            return Ok();
        }

        [Route("update")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> updateTipoMaterialesAsync([FromBody] UpdateTipoMaterialCommand command)
        {
            bool commandResult = false;

            commandResult = await _mediator.Send(command);

            return Ok();
        }

    }
}