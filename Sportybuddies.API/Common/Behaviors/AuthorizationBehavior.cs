using Sportybuddies.API.Common.Patterns;

namespace Sportybuddies.API.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>(IAuthorizationService authorizationService) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequireAuthorization, IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authResult = await authorizationService.AuthorizeAsync(request);
        
        if (authResult.IsFailure)
        {
            throw new ForbiddenException(authResult.Error);
        }

        return await next();
    }
}