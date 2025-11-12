using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.Client.AddClient;
using ProjektIznynierski.Application.Commands.Client.DeleteClient;
using ProjektIznynierski.Application.Commands.Client.UpdateClient;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.Client.GetAllClients;
using ProjektIznynierski.Application.Queries.Client.GetClientById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : BaseController
    {
        public ClientController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Clients", Description = "Retrieves a list of all clients.")]
        [ProducesResponseType(typeof(IEnumerable<ClientDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllClientsQuery ());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Client by ID", Description = "Retrieves a specific client by their ID.")]
        [ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetClientByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new client", Description = "Creates a new client with the provided details.")]
        [ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] AddClientCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing client", Description = "Updates an existing client with the provided details.")]
        [ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClientCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a client", Description = "Deletes a specific client by their ID.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteClientCommand { Id = id });
            return NoContent();
        }
    }
}
