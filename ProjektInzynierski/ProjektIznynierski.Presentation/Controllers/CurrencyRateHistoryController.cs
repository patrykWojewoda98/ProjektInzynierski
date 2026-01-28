using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.CurrencyRateHistory.AddCurrencyRateHistory;
using ProjektIznynierski.Application.Commands.CurrencyRateHistory.DeleteCurrencyRateHistory;
using ProjektIznynierski.Application.Commands.CurrencyRateHistory.UpdateCurrencyRateHistory;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.CurrencyRateHistory.GetCurrencyRateHistoryByPairId;
using ProjektIznynierski.Application.Queries.CurrencyRateHistory.GetLatestRate;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class CurrencyRateHistoryController : BaseController
    {
        public CurrencyRateHistoryController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet("by-pair/{currencyPairId}")]
        [SwaggerOperation(
            Summary = "Get currency rate history by currency pair",
            Description = "Retrieves historical currency rates for a given currency pair."
        )]
        [ProducesResponseType(typeof(IEnumerable<CurrencyRateHistoryDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByCurrencyPairId(int currencyPairId)
        {
            var result = await _mediator.Send(
                new GetCurrencyRateHistoryByPairIdQuery(currencyPairId)
            );
            return Ok(result);
        }

        [HttpGet("latest/{currencyPairId}")]
        [SwaggerOperation(
            Summary = "Get latest currency rate",
            Description = "Retrieves the latest currency rate for a given currency pair."
        )]
        [ProducesResponseType(typeof(CurrencyRateHistoryDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetLatest(int currencyPairId)
        {
            var result = await _mediator.Send(
                new GetLatestCurrencyRateQuery(currencyPairId)
            );
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new currency rate history entry",
            Description = "Creates a new currency rate history record."
        )]
        [ProducesResponseType(typeof(CurrencyRateHistoryDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] AddCurrencyRateHistoryCommand command
        )
        {
            var result = await _mediator.Send(command);

            return Created(string.Empty, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an existing currency rate history entry",
            Description = "Updates an existing currency rate history record."
        )]
        [ProducesResponseType(typeof(CurrencyRateHistoryDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateCurrencyRateHistoryCommand command
        )
        {
            if (id != command.Id)
            {
                return BadRequest(
                    "ID in the URL does not match the ID in the request body."
                );
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a currency rate history entry",
            Description = "Deletes a specific currency rate history record."
        )]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(
                new DeleteCurrencyRateHistoryCommand { Id = id }
            );
            return NoContent();
        }
    }
}
