using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.Sector.GetAllSectrors;
using ProjektIznynierski.Application.Queries.Sector.GetSectorById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class SectorController : BaseController
    {
        public SectorController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Sectors", Description = "Retrieves a list of all sectors.")]
        [ProducesResponseType(typeof(IEnumerable<SectorDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllSectorsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Secctor by ID", Description = "Retrieves a specific sector by its ID.")]
        [ProducesResponseType(typeof(SectorDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetSectorByIdQuery(id));
            return Ok(result);
        }
    }
}
