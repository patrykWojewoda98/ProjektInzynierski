using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.Region.GetAllRegions;
using ProjektIznynierski.Application.Queries.Region.GetRegionById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class RegionController : BaseController
    {
        public RegionController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Regions", Description = "Retrieves a list of all regions.")]
        [ProducesResponseType(typeof(IEnumerable<RegionDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllRegionsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Region by ID", Description = "Retrieves a specific region by its ID.")]
        [ProducesResponseType(typeof(RegionDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetRegionByIdQuery(id));
            return Ok(result);
        }
    }
}
