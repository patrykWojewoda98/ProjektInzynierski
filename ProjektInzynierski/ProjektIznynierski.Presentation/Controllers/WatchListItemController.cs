using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.Client.AddClient;
using ProjektIznynierski.Application.Commands.Client.DeleteClient;
using ProjektIznynierski.Application.Commands.Client.UpdateClient;
using ProjektIznynierski.Application.Commands.WatchListItem.CreateWatchListItem;
using ProjektIznynierski.Application.Commands.WatchListItem.DeleteWatchListItem;
using ProjektIznynierski.Application.Commands.WatchListItem.UpdateWatchListItem;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.WatchList.GetAllWatchLists;
using ProjektIznynierski.Application.Queries.WatchList.GetWatchListById;
using ProjektIznynierski.Application.Queries.WatchListItem.GetAllWatchListItems;
using ProjektIznynierski.Application.Queries.WatchListItem.GetAllWatchListItemsByClientId;
using ProjektIznynierski.Application.Queries.WatchListItem.GetWatchListItemById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class WatchListItemController : BaseController
    {
        public WatchListItemController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Watch List Items", Description = "Retrieves a list of all Watch List Items.")]
        [ProducesResponseType(typeof(IEnumerable<WatchListItemDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllWatchListItemsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Watch List Item by ID", Description = "Retrieves a specific Watch List Item by its ID.")]
        [ProducesResponseType(typeof(WatchListItemDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetWatchListItemByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("client/{clientId}")]
        [SwaggerOperation(Summary = "Get Watch List Items by ClientID", Description = "Retrieves all watch list items ClientID.")]
        [ProducesResponseType(typeof(WatchListItemDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetClientWatchListItems([FromRoute] int clientId)
        {
            var result = await _mediator.Send(new GetAllWatchListItemsByClientIdQuery(clientId));
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Watch List Item", Description = "Creates a new Watch List Item with the provided details.")]
        [ProducesResponseType(typeof(WatchListItemDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateWatchListItemCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing Watch List Item", Description = "Updates an existing Watch List Item with the provided details.")]
        [ProducesResponseType(typeof(WatchListItemDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWatchListItemCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a Watch List Item", Description = "Deletes a specific Watch List Item by their ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteWatchListItemCommand { Id = id });
            return NoContent();
        }

    }
}
