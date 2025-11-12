using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.MarketData.GetAllMarketData;
using ProjektIznynierski.Application.Queries.MarketData.GetMarketDataById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class MarketDataController : BaseController
    {
        public MarketDataController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Market Data", Description = "Retrieves a list of all market data.")]
        [ProducesResponseType(typeof(IEnumerable<MarketDataDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllMarketDataQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Market Data by ID", Description = "Retrieves a specific market data entry by its ID.")]
        [ProducesResponseType(typeof(MarketDataDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetMarketDataByIdQuery(id));
            return Ok(result);
        }
    }
}
