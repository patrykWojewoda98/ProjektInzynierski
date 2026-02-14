using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProjektIznynierski.Application.Commands.ClientInterfaceConfig.CreateClientInterfaceConfig;
using ProjektIznynierski.Application.Commands.ClientInterfaceConfig.DeleteClientInterfaceConfig;
using ProjektIznynierski.Application.Commands.ClientInterfaceConfig.ReorderClientInterfaceConfig;
using ProjektIznynierski.Application.Commands.ClientInterfaceConfig.ToggleVisibilityClientInterfaceConfig;
using ProjektIznynierski.Application.Commands.ClientInterfaceConfig.UpdateClientInterfaceConfig;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.ClientInterfaceConfig.GetClientConfigForEmployee;
using ProjektIznynierski.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    /// <summary>
    /// Employee-only: manage Client interface texts and menu configuration.
    /// Requires Employee authorization (send Bearer token from employee login).
    /// </summary>
    [Route("api/EmployeeClientConfig")]
    [ApiController]
    public class EmployeeClientConfigController : ControllerBase
    {
        private static readonly string[] AllowedImageExtensions = { ".png", ".jpg", ".jpeg", ".gif", ".webp", ".svg" };
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public EmployeeClientConfigController(IMediator mediator, IWebHostEnvironment env, IConfiguration configuration)
        {
            _mediator = mediator;
            _env = env;
            _configuration = configuration;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all config items (including hidden)", Description = "Returns all Client interface config items for the given platform. Employee auth required.")]
        [ProducesResponseType(typeof(List<ClientInterfaceConfigDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] string platform)
        {
            if (string.IsNullOrEmpty(platform) || !Enum.TryParse<ClientInterfacePlatform>(platform, ignoreCase: true, out var platformEnum))
                return BadRequest("platform must be 'Mobile' or 'Web'.");

            var result = await _mediator.Send(new GetClientConfigForEmployeeQuery { Platform = platformEnum });
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get config item by ID")]
        [ProducesResponseType(typeof(ClientInterfaceConfigDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var allMobile = await _mediator.Send(new GetClientConfigForEmployeeQuery { Platform = ClientInterfacePlatform.Mobile });
            var allWeb = await _mediator.Send(new GetClientConfigForEmployeeQuery { Platform = ClientInterfacePlatform.Web });
            var item = allMobile.Concat(allWeb).FirstOrDefault(x => x.Id == id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create config item")]
        [ProducesResponseType(typeof(ClientInterfaceConfigDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateClientInterfaceConfigCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update config item")]
        [ProducesResponseType(typeof(ClientInterfaceConfigDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClientInterfaceConfigCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID in URL does not match request body.");
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex) when (ex.Message.Contains("not found"))
            {
                return NotFound();
            }
        }

        [HttpPatch("reorder")]
        [SwaggerOperation(Summary = "Reorder items")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Reorder([FromBody] ReorderClientInterfaceConfigCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("{id}/toggle-visibility")]
        [SwaggerOperation(Summary = "Toggle visibility")]
        [ProducesResponseType(typeof(ClientInterfaceConfigDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ToggleVisibility(int id)
        {
            try
            {
                var result = await _mediator.Send(new ToggleVisibilityClientInterfaceConfigCommand { Id = id });
                return Ok(result);
            }
            catch (Exception ex) when (ex.Message.Contains("not found"))
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete config item")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _mediator.Send(new DeleteClientInterfaceConfigCommand { Id = id });
                return NoContent();
            }
            catch (Exception ex) when (ex.Message.Contains("not found"))
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Upload an image for use in Client menu. File is saved to Frontend/assets/images (Mobile) or Fontend Web/assets/images (Web).
        /// Returns the path string to store in ClientInterfaceConfig.ImagePath (e.g. "assets/images/filename.png").
        /// </summary>
        [HttpPost("upload-image")]
        [SwaggerOperation(Summary = "Upload menu image")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UploadImage([FromQuery] string platform, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            if (string.IsNullOrEmpty(platform) || !Enum.TryParse<ClientInterfacePlatform>(platform, ignoreCase: true, out var platformEnum))
                return BadRequest("platform must be 'Mobile' or 'Web'.");

            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(ext) || !AllowedImageExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
                return BadRequest("Allowed image extensions: " + string.Join(", ", AllowedImageExtensions));

            var relativePath = platformEnum == ClientInterfacePlatform.Mobile
                ? _configuration["ClientConfig:MobileAssetsPath"]
                : _configuration["ClientConfig:WebAssetsPath"];

            var baseDir = string.IsNullOrEmpty(relativePath)
                ? Path.Combine(_env.ContentRootPath, "client-config-uploads", platformEnum.ToString().ToLowerInvariant())
                : Path.GetFullPath(Path.Combine(_env.ContentRootPath, relativePath));
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);

            var fileName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(baseDir, fileName);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
                await file.CopyToAsync(stream);

            var pathToStore = Path.Combine("assets", "images", fileName).Replace('\\', '/');
            return Ok(new { imagePath = pathToStore });
        }
    }
}
