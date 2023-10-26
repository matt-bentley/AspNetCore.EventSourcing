using MediatR;
using AspNetCore.EventSourcing.Api.Infrastructure.ActionResults;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.EventSourcing.Core.Accounts.ReadModels;
using AspNetCore.EventSourcing.Application.Accounts.Queries;
using AspNetCore.EventSourcing.Application.Accounts.Models;
using AspNetCore.EventSourcing.Application.Accounts.Commands;

namespace AspNetCore.EventSourcing.Api.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    [Produces("application/json")]
    public sealed class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AccountReadModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var account = await _mediator.Send(new GetAccountQuery(id));
            return Ok(account);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<AccountReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] Guid? customerId)
        {
            var accounts = await _mediator.Send(new GetAccountsQuery(customerId));
            return Ok(accounts);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreatedResultEnvelope), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] AccountCreateDto account)
        {
            var id = Guid.NewGuid();
            await _mediator.Send(new OpenAccountCommand(id, account.CustomerId, account.AccountNumber));
            return CreatedAtAction(nameof(Get), new { id }, new CreatedResultEnvelope(id));
        }

        [HttpPut("{id}/deposit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deposit(Guid id, [FromBody] TransactionCreateDto transaction)
        {
            await _mediator.Send(new DepositAmountCommand(id, transaction.Amount, transaction.Description));
            return NoContent();
        }

        [HttpPut("{id}/withdraw")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Withdraw(Guid id, [FromBody] TransactionCreateDto transaction)
        {
            await _mediator.Send(new WithdrawAmountCommand(id, transaction.Amount, transaction.Description));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new CloseAccountCommand(id));
            return NoContent();
        }
    }
}