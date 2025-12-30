using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.Currency.CreateCurrency;
using ProjektIznynierski.Application.Commands.Currency.DeleteCurrency;
using ProjektIznynierski.Application.Commands.Currency.UpdateCurrency;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.Currency.GetCurrencies;
using ProjektIznynierski.Application.Queries.Currency.GetCurrencyById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class CurrencyController : BaseController
    {
        public CurrencyController(IMediator mediator) : base(mediator)
        {
        }


        [HttpGet]
        [SwaggerOperation(Summary = "Get Currencies", Description = "Retrieves a list of available currencies.")]
        [ProducesResponseType(typeof(IEnumerable<CurrencyDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllCurenciesQuery());
            return Ok(result);
        }


        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Currency by ID", Description = "Retrieves a specific currency by its ID.")]
        [ProducesResponseType(typeof(CurrencyDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetCurrencyByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Currency", Description = "Creates a new Currency with the provided details.")]
        [ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody]CreateCurrencyCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing Currency", Description = "Updates an existing Currency with the provided details.")]
        [ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCurrencyCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a client", Description = "Deletes a specific Currency by their ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteCurrencyCommand { Id = id });
            return NoContent();
        }

    }
}