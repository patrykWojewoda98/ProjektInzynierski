using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.ClientInterfaceConfig.GetClientMenuConfig;
using ProjektIznynierski.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    /// <summary>
    /// Client-facing endpoint: returns menu configuration for the Client interface (visible items only, ordered).
    /// Used by Frontend (Mobile) and FrontendWeb (Web) to render the main menu.
    /// </summary>
    [Route("api/client-config")]
    [ApiController]
    public class ClientConfigController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientConfigController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("menu")]
        [SwaggerOperation(Summary = "Get client menu", Description = "Returns ordered and visible menu items for the Client interface. Platform: Mobile or Web.")]
        [ProducesResponseType(typeof(List<ClientMenuConfigItemDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetMenu([FromQuery] string platform)
        {
            if (string.IsNullOrEmpty(platform) || !Enum.TryParse<ClientInterfacePlatform>(platform, ignoreCase: true, out var platformEnum))
                return BadRequest("platform must be 'Mobile' or 'Web'.");

            var result = await _mediator.Send(new GetClientMenuConfigQuery { Platform = platformEnum });
            return Ok(result);
        }
    }
}
