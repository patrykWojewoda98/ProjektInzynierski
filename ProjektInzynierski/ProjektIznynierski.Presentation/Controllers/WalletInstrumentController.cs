using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.WalletInstrument.CreateWalletInstrument;
using ProjektIznynierski.Application.Commands.WalletInstrument.DeleteWalletInstrument;
using ProjektIznynierski.Application.Commands.WalletInstrument.UpdateWalletInstrument;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.WalletInstrument.GetAllWalletInstruments;
using ProjektIznynierski.Application.Queries.WalletInstrument.GetWalletInstrumentById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class WalletInstrumentController : BaseController
    {
        public WalletInstrumentController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Wallet Instruments", Description = "Retrieves a list of all wallet instruments.")]
        [ProducesResponseType(typeof(IEnumerable<WalletInstrumentDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllWalletInstrumentsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Wallet Instrument by ID", Description = "Retrieves a specific wallet instrument by its ID.")]
        [ProducesResponseType(typeof(WalletInstrumentDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetWalletInstrumentByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("wallet/{walletId}")]
        [SwaggerOperation(Summary = "Get Wallet Instruments by Wallet ID",Description = "Retrieves all investment instruments assigned to the specified wallet.")]
        [ProducesResponseType(typeof(IEnumerable<WalletInstrumentDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByWalletId(int walletId)
        {
            var result = await _mediator.Send(
                new GetWalletInstrumentsByWalletIdQuery(walletId));

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Wallet Instrument", Description = "Creates a new Wallet Instrument with the provided details.")]
        [ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateWalletInstrumentCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing Wallet Instrument", Description = "Updates an existing Wallet Instrument with the provided details.")]
        [ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWalletInstrumentCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a client", Description = "Deletes a specific Wallet Instrument by their ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteWalletInstrumentCommand { Id = id });
            return NoContent();
        }
    }
}
