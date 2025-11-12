using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.InvestProfile.GetAllInvestProfiles;
using ProjektIznynierski.Application.Queries.InvestProfile.GetInvestProfileById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class InvestProfileController : BaseController
    {
        public InvestProfileController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Investment Profiles", Description = "Retrieves a list of all investment profiles.")]
        [ProducesResponseType(typeof(IEnumerable<InvestProfileDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllInvestProfilesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Investment Profile by ID", Description = "Retrieves a specific investment profile by its ID.")]
        [ProducesResponseType(typeof(InvestProfileDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetInvestProfileByIdQuery(id));
            return Ok(result);
        }
    }
}
