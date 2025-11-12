using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.TradeHistory.GetAllTradeHistories;
using ProjektIznynierski.Application.Queries.TradeHistory.GetTradeHistoryById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class TradeHistoryController : BaseController
    {
        public TradeHistoryController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Trade Histories", Description = "Retrieves a list of all trade histories.")]
        [ProducesResponseType(typeof(IEnumerable<TradeHistoryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllTradeHistoriesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Trade History by ID", Description = "Retrieves a specific trade history by its ID.")]
        [ProducesResponseType(typeof(TradeHistoryDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetTradeHistoryByIdQuery(id));
            return Ok(result);
        }
    }
}
