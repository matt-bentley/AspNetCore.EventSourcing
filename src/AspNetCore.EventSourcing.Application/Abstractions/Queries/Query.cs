using MediatR;

namespace AspNetCore.EventSourcing.Application.Abstractions.Queries
{
    public abstract record Query<T> : IRequest<T>;
}
