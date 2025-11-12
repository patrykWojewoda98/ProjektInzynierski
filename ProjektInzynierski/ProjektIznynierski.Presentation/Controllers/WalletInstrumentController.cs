using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    }
}
