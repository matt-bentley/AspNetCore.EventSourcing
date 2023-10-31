using MediatR;
using AspNetCore.EventSourcing.Api.Infrastructure.ActionResults;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.EventSourcing.Application.Customers.Models;
using AspNetCore.EventSourcing.Application.Customers.Queries;
using AspNetCore.EventSourcing.Application.Customers.Commands;

namespace AspNetCore.EventSourcing.Api.Controllers
{
    [ApiController]
    [Route("api/customers")]
    [Produces("application/json")]
    public sealed class CustomersController : ControllerBase
    {
        private readonly ISender _mediator;

        public CustomersController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var customer = await _mediator.Send(new GetCustomerQuery(id));
            return Ok(customer);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CustomerDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var customers = await _mediator.Send(new GetCustomersQuery());
            return Ok(customers);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreatedResultEnvelope), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CustomerCreateDto customer)
        {
            var id = Guid.NewGuid();
            await _mediator.Send(new CreateCustomerCommand(id, customer.FirstName, customer.LastName, customer.Email));
            return CreatedAtAction(nameof(Get), new { id }, new CreatedResultEnvelope(id));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(Guid id, [FromBody] CustomerUpdateDto customer)
        {
            await _mediator.Send(new UpdateCustomerNameCommand(id, customer.FirstName, customer.LastName));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new CustomerLeaveCommand(id));
            return NoContent();
        }
    }
}