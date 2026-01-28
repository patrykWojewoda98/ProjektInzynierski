using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.CurrencyPair.AddCurrencyPair;
using ProjektIznynierski.Application.Commands.CurrencyPair.DeleteCurrencyPair;
using ProjektIznynierski.Application.Commands.CurrencyPair.UpdateCurrencyPair;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.CurrencyPair.GetAllCurrencyPairs;
using ProjektIznynierski.Application.Queries.CurrencyPair.GetById;
using ProjektIznynierski.Application.Queries.CurrencyPair.GetByCurrencies;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class CurrencyPairController : BaseController
    {
        public CurrencyPairController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all currency pairs",
            Description = "Retrieves a list of all currency pairs."
        )]
        [ProducesResponseType(typeof(IEnumerable<CurrencyPairDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(
                new GetAllCurrencyPairsQuery()
            );
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get currency pair by ID",
            Description = "Retrieves a specific currency pair by its ID."
        )]
        [ProducesResponseType(typeof(CurrencyPairDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(
                new GetCurrencyPairByIdQuery(id)
            );
            return Ok(result);
        }

        [HttpGet("by-currencies")]
        [SwaggerOperation(
            Summary = "Get currency pair by currencies",
            Description = "Retrieves a currency pair by base and quote currency IDs."
        )]
        [ProducesResponseType(typeof(CurrencyPairDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByCurrencies(
            [FromQuery] int baseCurrencyId,
            [FromQuery] int quoteCurrencyId
        )
        {
            var result = await _mediator.Send(
                new GetCurrencyPairByCurrenciesQuery(
                    baseCurrencyId,
                    quoteCurrencyId
                )
            );
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new currency pair",
            Description = "Creates a new currency pair."
        )]
        [ProducesResponseType(typeof(CurrencyPairDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] AddCurrencyPairCommand command
        )
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result
            );
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an existing currency pair",
            Description = "Updates an existing currency pair."
        )]
        [ProducesResponseType(typeof(CurrencyPairDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateCurrencyPairCommand command
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
            Summary = "Delete a currency pair",
            Description = "Deletes a specific currency pair by its ID."
        )]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(
                new DeleteCurrencyPairCommand { Id = id }
            );
            return NoContent();
        }
    }
}