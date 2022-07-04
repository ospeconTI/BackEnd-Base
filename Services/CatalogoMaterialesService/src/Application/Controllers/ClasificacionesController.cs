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
    public class ClasificacionesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ClasificacionesController> _logger;

        private readonly IClasificacionesQueries _clasificacionesQueries;

        public ClasificacionesController(
            IMediator mediator,
            ILogger<ClasificacionesController> logger,
            IClasificacionesQueries clasificaciones)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _clasificacionesQueries = clasificaciones ?? throw new ArgumentNullException(nameof(clasificaciones));
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetClasificacionesAsync(Guid id)
        {
            try
            {
                //Todo: It's good idea to take advantage of GetOrderByIdQuery and handle by GetCustomerByIdQueryHandler
                //var order customer = await _mediator.Send(new GetOrderByIdQuery(orderId));
                var sector = await _clasificacionesQueries.GetClasificacionesAsync(id);

                return Ok(sector);
            }
            catch
            {
                return NotFound();
            }
        }


        [Route("getByName/{descripcion}")]
        [HttpGet]
        public async Task<ActionResult> GetClasificacionesByNameAsync(string descripcion)
        {
            try
            {
                //Todo: It's good idea to take advantage of GetOrderByIdQuery and handle by GetCustomerByIdQueryHandler
                //var order customer = await _mediator.Send(new GetOrderByIdQuery(orderId));
                var legajo = await _clasificacionesQueries.GetClasificacionesByNameAsync(descripcion);

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
                var clasificaciones = await _clasificacionesQueries.GetAll();

                return Ok(clasificaciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> addClasificacionessAsync([FromBody] AddClasificacionCommand command)
        {

            Guid UID = await _mediator.Send(command);

            return Ok(UID);
        }

        [Route("update")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> updateClasificacionesAsync([FromBody] UpdateClasificacionCommand command)
        {
            bool commandResult = false;

            commandResult = await _mediator.Send(command);

            return Ok();
        }

    }
}