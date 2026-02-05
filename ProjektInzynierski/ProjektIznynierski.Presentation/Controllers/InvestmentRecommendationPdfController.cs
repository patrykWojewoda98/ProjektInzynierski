using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.Pdf;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class InvestmentRecommendationPdfController : BaseController
    {
        public InvestmentRecommendationPdfController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{analysisRequestId:int}")]
        [SwaggerOperation(
            Summary = "Generate investment recommendation PDF",
            Description = "Generates a PDF file with investment recommendation based on AI analysis."
        )]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Generate(int analysisRequestId)
        {
            var pdfBytes = await _mediator.Send(
                new GenerateInvestmentRecommendationPdfCommand(analysisRequestId));

            if (pdfBytes == null || pdfBytes.Length == 0)
                return NotFound();

            var fileName = $"Investment_Recommendation_{analysisRequestId}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}
