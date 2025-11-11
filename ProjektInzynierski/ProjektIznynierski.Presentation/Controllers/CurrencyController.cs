using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.Currency.GetCurrencies;
using ProjektIznynierski.Application.Queries.Currency.GetCurrencyById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class CurrexncyController : BaseController
    {
        public CurrexncyController(IMediator mediator) : base(mediator)
        {
        }


        [HttpGet]
        [SwaggerOperation(Summary = "Get Currencies", Description = "Retrieves a list of available currencies.")]
        [ProducesResponseType(typeof(IEnumerable<CurrencyDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllCurenciesQuery());
            return Ok(result);
        }


        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Currency by ID", Description = "Retrieves a specific currency by its ID.")]
        [ProducesResponseType(typeof(CurrencyDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetCurrencyByIdQuery(id));
            return Ok(result);
        }



    }
}