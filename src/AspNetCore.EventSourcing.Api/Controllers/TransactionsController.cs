using MediatR;
using AspNetCore.EventSourcing.Api.Infrastructure.ActionResults;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.EventSourcing.Core.Accounts.ReadModels;
using AspNetCore.EventSourcing.Application.Accounts.Queries;

namespace AspNetCore.EventSourcing.Api.Controllers
{
    [ApiController]
    [Route("api/accounts/{accountId}/transactions")]
    [Produces("application/json")]
    public sealed class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TransactionReadModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid accountId)
        {
            var transactions = await _mediator.Send(new GetTransactionsQuery(accountId));
            return Ok(transactions);
        }
    }
}