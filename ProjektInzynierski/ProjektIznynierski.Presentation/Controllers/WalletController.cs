using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.Wallet.GetAllWallets;
using ProjektIznynierski.Application.Queries.Wallet.GetWalletById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class WalletController : BaseController
    {
        public WalletController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Wallets", Description = "Retrieves a list of all wallets.")]
        [ProducesResponseType(typeof(IEnumerable<WalletDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllWalletsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Wallet by ID", Description = "Retrieves a specific wallet by its ID.")]
        [ProducesResponseType(typeof(WalletDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetWalletByIdQuery(id));
            return Ok(result);
        }
    }
}
