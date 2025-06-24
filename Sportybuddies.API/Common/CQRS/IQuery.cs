using MediatR;

namespace Sportybuddies.API.Common.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : notnull
{
}