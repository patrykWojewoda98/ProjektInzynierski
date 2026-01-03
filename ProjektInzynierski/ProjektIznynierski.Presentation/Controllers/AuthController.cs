using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.Auth.Login;
using ProjektIznynierski.Application.Commands.AuthAdmin.Login;
using ProjektIznynierski.Application.Commands.AuthEmployee;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Authenticate client and return JWT token.")]
        [ProducesResponseType(typeof(LoginResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login-employee")]
        [SwaggerOperation(Summary = "Authenticate eployee and return JWT token.")]
        [ProducesResponseType(typeof(LoginResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LoginEmployee([FromBody] LoginEmployeeCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("verify-employee-2fa")]
        [SwaggerOperation(
            Summary = "Verify employee 2FA code",
            Description = "Verifies the 2FA code and returns JWT token.")]
        [ProducesResponseType(typeof(LoginResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> VerifyEmployee2FA([FromBody] VerifyEmployee2FACommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
