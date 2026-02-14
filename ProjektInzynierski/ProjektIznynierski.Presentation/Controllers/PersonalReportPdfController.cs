using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.Pdf;
using ProjektIznynierski.Application.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class PersonalReportPdfController : BaseController
    {
        public PersonalReportPdfController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("generate")]
        [SwaggerOperation(
            Summary = "Generate personal report PDF",
            Description = "Generates a customizable PDF report with selected sections: instrument info, financial metrics, financial reports, AI recommendation, portfolio composition."
        )]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Generate([FromBody] GeneratePersonalReportRequest request)
        {
            if (request == null)
                return BadRequest("Request body is required.");

            var command = new GeneratePersonalReportPdfCommand(
                request.ClientId,
                request.InvestInstrumentId,
                request.IncludeInstrumentInfo,
                request.IncludeFinancialMetrics,
                request.IncludedMetricFields,
                request.IncludeFinancialReports,
                request.IncludedFinancialReportIds,
                request.IncludePortfolioComposition,
                request.CustomIntroText,
                request.CustomOutroText,
                request.FontFamily,
                request.FontSize);

            var pdfBytes = await _mediator.Send(command);

            if (pdfBytes == null || pdfBytes.Length == 0)
                return NotFound();

            var fileName = $"Personal_Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}
