using MediatR;

namespace AspNetCore.EventSourcing.Application.Abstractions.Commands
{
    public abstract record Command : IRequest<Unit>;
}
