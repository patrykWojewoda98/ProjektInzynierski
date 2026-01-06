using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.FinancialMetric.CreateFinancialMetric;
using ProjektIznynierski.Application.Commands.InvestInstrument.CreateInvestInstrument;
using ProjektIznynierski.Application.Commands.InvestInstrument.DeleteInvestInstrument;
using ProjektIznynierski.Application.Commands.InvestInstrument.UpdateInvestInstrument;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.InvestInstrument.GetAllByRegionId;
using ProjektIznynierski.Application.Queries.InvestInstrument.GetInvestInstrumentById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

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

        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Get investment instrument by ID")]
        [ProducesResponseType(typeof(InvestInstrumentDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetInvestInstrumentByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("region/{regionId:int}")]
        [SwaggerOperation(Summary = "Get investment instruments by region ID")]
        [ProducesResponseType(typeof(IEnumerable<InvestInstrumentDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByRegionId(int regionId)
        {
            var result = await _mediator.Send(new GetAllByRegionIdQuery(regionId));
            return Ok(result);
        }

        [HttpGet("type/{typeId:int}")]
        [SwaggerOperation(Summary = "Get investment instruments by type ID")]
        [ProducesResponseType(typeof(IEnumerable<InvestInstrumentDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByTypeId(int typeId)
        {
            var result = await _mediator.Send(new GetAllByTypeIdQuery(typeId));
            return Ok(result);
        }

        [HttpGet("sector/{sectorId:int}")]
        [SwaggerOperation(Summary = "Get investment instruments by sector ID")]
        [ProducesResponseType(typeof(IEnumerable<InvestInstrumentDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBySectorId(int sectorId)
        {
            var result = await _mediator.Send(new GetByAllSectorIdQuery(sectorId));
            return Ok(result);
        }


        [HttpPost]
        [SwaggerOperation(Summary = "Create a new client", Description = "Creates a new client with the provided details.")]
        [ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateInvestInstrumentCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPost("{instrumentId}/FinancialMetric")]
        [SwaggerOperation(Summary = "Create financial metric for investment instrument",Description = "Creates and assigns a financial metric to the given investment instrument.")]
        [ProducesResponseType(typeof(FinancialMetricDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateFinancialMetric(int instrumentId,[FromBody] CreateFinancialMetricAndAssignToInstrumentCommand command)
        {
            command = command with { InvestInstrumentId = instrumentId };   
            var result = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(FinancialMetricController.GetById),
                "FinancialMetric",
                new { id = result.Id },
                result
            );
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing Investment Instrument", Description = "Updates an existing Investment Instrument with the provided details.")]
        [ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInvestInstrumentCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a Investment Instrument", Description = "Deletes a specific Investment Instrument by their ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteInvestInstrumentCommand { Id = id });
            return NoContent();
        }
    }
}
