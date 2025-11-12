using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.FinancialReport.GetAllFinancialReports;
using ProjektIznynierski.Application.Queries.FinancialReport.GetFinancialReportById;
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
    }
}
