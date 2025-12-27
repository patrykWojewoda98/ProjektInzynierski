using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.FinancialReport.CreateFinancialReport;
using ProjektIznynierski.Application.Commands.FinancialReport.DeleteFinancialReport;
using ProjektIznynierski.Application.Commands.FinancialReport.ImportFinancialReports;
using ProjektIznynierski.Application.Commands.FinancialReport.UpdateFinancialReport;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.FinancialReport.GetAllFinancialReports;
using ProjektIznynierski.Application.Queries.FinancialReport.GetFinancialReportById;
using ProjektIznynierski.Application.Queries.FinancialReport.GetFinancialReportsByInvestInstrumentID;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class FinancialReportController : BaseController
    {
        public FinancialReportController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Financial Reports", Description = "Retrieves a list of all financial reports.")]
        [ProducesResponseType(typeof(IEnumerable<FinancialReportDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllFinancialReportsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Financial Report by ID", Description = "Retrieves a specific financial report by its ID.")]
        [ProducesResponseType(typeof(FinancialReportDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetFinancialReportByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("invest-instrument/{id}")]
        [SwaggerOperation(Summary = "Get Financial Report by Invest Instrument ID", Description = "Retrieves a specific financial report by Invest Instrument ID.")]
        [ProducesResponseType(typeof(FinancialReportDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByIdInvestInstrument(int id)
        {
            var result = await _mediator.Send(new GetFinancialReportsByInvestInstrumentIDQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Financial Report", Description = "Creates a new Financial Report with the provided details.")]
        [ProducesResponseType(typeof(FinancialReportDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateFinancialReportCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPost("import")]
        [SwaggerOperation(Summary = "Import financial reports by ISIN", Description = "Automatically imports all available historical financial reports for a given ISIN.")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ImportByIsin(
        [FromBody] ImportFinancialReportsCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.Isin))
                return BadRequest("ISIN is required.");

            var importedCount = await _mediator.Send(
                new ImportFinancialReportsCommand
                {
                    Isin = request.Isin
                });

            return Ok(new
            {
                importedReports = importedCount
            });
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing Financial Report", Description = "Updates an existing Financial Report with the provided details.")]
        [ProducesResponseType(typeof(FinancialReportDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFinancialReportCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a Financial Report", Description = "Deletes a specific Financial Report by their ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteFinancialReportCommand { Id = id });
            return NoContent();
        }

        
    }
}
