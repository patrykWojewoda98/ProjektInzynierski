using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektIznynierski.Application.Commands.Employee.AddEmployee;
using ProjektIznynierski.Application.Commands.Employee.DeleteEmployee;
using ProjektIznynierski.Application.Commands.Employee.UpdateEmployee;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.Employee.GetAllEmployees;
using ProjektIznynierski.Application.Queries.Employee.GetEmployeeById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProjektIznynierski.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeController : BaseController
    {
        public EmployeeController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get Employees",
            Description = "Retrieves a list of all employees."
        )]
        [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllEmployeesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get Employee by ID",
            Description = "Retrieves a specific employee by their ID."
        )]
        [ProducesResponseType(typeof(EmployeeDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetEmployeeByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new employee",
            Description = "Creates a new employee with the provided details."
        )]
        [ProducesResponseType(typeof(EmployeeDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] AddEmployeeCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing employee",Description = "Updates an existing employee with the provided details.")]
        [ProducesResponseType(typeof(EmployeeDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateEmployeeCommand command)
        {
            if (command is null)
                return BadRequest("Request body is missing.");

            command.Id = id;

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete an employee",
            Description = "Deletes a specific employee by their ID."
        )]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteEmployeeCommand { Id = id });
            return NoContent();
        }
    }
}
