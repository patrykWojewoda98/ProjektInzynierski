using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.MarketData.DeleteMarketData;
using ProjektIznynierski.Application.Commands.MarketData.ImportMarketData;
using ProjektIznynierski.Application.Commands.MarketData.UpdateMarketData;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.MarketData.GetAllMarketData;
using ProjektIznynierski.Application.Queries.MarketData.GetMarketDataById;
using ProjektIznynierski.Application.Queries.MarketData.GetMarketDataByInvestInstrumentId;
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

        [HttpGet("invest-instrument/{investInstrumentID:int}")]
        [SwaggerOperation(Summary = "Get Market Data by instruments by Invest Instrument ID")]
        [ProducesResponseType(typeof(IEnumerable<InvestInstrumentDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByRegionId(int investInstrumentID)
        {
            var result = await _mediator.Send(new GetMarketDataByInvestInstrumentIdQuery(investInstrumentID));
            return Ok(result);
        }



        [HttpPost("import")]
        [SwaggerOperation(Summary = "Import market data by Ticker", Description = "Imports market data for a given Ticker.")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ImportByTicker([FromBody] ImportMarketDataCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.Ticker))
                return BadRequest("TIcker is required.");

            var importedCount = await _mediator.Send(
                new ImportMarketDataCommand { Ticker = request.Ticker });

            return Ok(new { importedMarketData = importedCount });
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update Market Data", Description = "Updates an existing market data entry.")]
        [ProducesResponseType(typeof(MarketDataDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMarketDataCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID in the URL does not match the ID in the request body.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete Market Data", Description = "Deletes a specific market data entry by its ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteMarketDataCommand { Id = id });
            return NoContent();
        }
    }
}
