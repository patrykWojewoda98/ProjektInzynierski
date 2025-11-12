using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.WatchList.GetAllWatchLists;
using ProjektIznynierski.Application.Queries.WatchList.GetWatchListById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class WatchListController : BaseController
    {
        public WatchListController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Watch Lists", Description = "Retrieves a list of all watch lists.")]
        [ProducesResponseType(typeof(IEnumerable<WatchListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllWatchListsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Watch List by ID", Description = "Retrieves a specific watch list by its ID.")]
        [ProducesResponseType(typeof(WatchListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetWatchListByIdQuery(id));
            return Ok(result);
        }
    }
}
