using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.Comment.AddComment;
using ProjektIznynierski.Application.Commands.Comment.DeleteComment;
using ProjektIznynierski.Application.Commands.Comment.UpdateComment;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.Comment.GetAllComments;
using ProjektIznynierski.Application.Queries.Comment.GetCommentById;
using ProjektIznynierski.Application.Queries.Comment.GetCommentsByClientId;
using ProjektIznynierski.Application.Queries.Comment.GetCommentsByInvestInstrumentId;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class CommentController : BaseController
    {
        public CommentController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all comments",
            Description = "Retrieves a list of all comments."
        )]
        [ProducesResponseType(typeof(IEnumerable<CommentDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllCommentsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get comment by ID",
            Description = "Retrieves a specific comment by its ID."
        )]
        [ProducesResponseType(typeof(CommentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetCommentByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("client/{clientId}")]
        [SwaggerOperation(
            Summary = "Get comments by client ID",
            Description = "Retrieves all comments created by a specific client."
        )]
        [ProducesResponseType(typeof(IEnumerable<CommentDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByClientId(int clientId)
        {
            var result = await _mediator.Send(
                new GetCommentsByClientIdQuery(clientId)
            );
            return Ok(result);
        }

        [HttpGet("instrument/{instrumentId}")]
        [SwaggerOperation(
            Summary = "Get comments by investment instrument ID",
            Description = "Retrieves all comments related to a specific investment instrument."
        )]
        [ProducesResponseType(typeof(IEnumerable<CommentDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByInvestInstrumentId(int instrumentId)
        {
            var result = await _mediator.Send(
                new GetCommentsByInvestInstrumentIdQuery(instrumentId)
            );
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new comment",
            Description = "Creates a new comment for an investment instrument."
        )]
        [ProducesResponseType(typeof(CommentDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] AddCommentCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an existing comment",
            Description = "Updates an existing comment."
        )]
        [ProducesResponseType(typeof(CommentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommentCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a comment",
            Description = "Deletes a specific comment by its ID."
        )]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteCommentCommand { Id = id });
            return NoContent();
        }
    }
}
