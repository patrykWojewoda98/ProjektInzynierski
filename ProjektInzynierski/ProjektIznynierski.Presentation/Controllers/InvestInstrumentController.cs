using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.InvestInstrument.GetAllInvestInstruments;
using ProjektIznynierski.Application.Queries.InvestInstrument.GetInvestInstrumentById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class InvestInstrumentController : BaseController
    {
        public InvestInstrumentController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Investment Instruments", Description = "Retrieves a list of all investment instruments.")]
        [ProducesResponseType(typeof(IEnumerable<InvestInstrumentDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllInvestInstrumentsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Investment Instrument by ID", Description = "Retrieves a specific investment instrument by its ID.")]
        [ProducesResponseType(typeof(InvestInstrumentDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetInvestInstrumentByIdQuery(id));
            return Ok(result);
        }
    }
}
