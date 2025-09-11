namespace Sportybuddies.API.Common.Patterns;

public interface IRequireAuthorization
{
    Guid GetResourceOwnerId();
}

public interface IAuthorizationService
{
    Task<Result> AuthorizeAsync<T>(T request) where T : IRequireAuthorization;
}

public class AuthorizationService(ICurrentUserProvider currentUserProvider) : IAuthorizationService
{
    public Task<Result> AuthorizeAsync<T>(T request) where T : IRequireAuthorization
    {
        var currentUserId = currentUserProvider.GetCurrentUserId();
        var resourceOwnerId = request.GetResourceOwnerId();
        
        if (currentUserId != resourceOwnerId)
        {
            return Task.FromResult(Result.Failure("You are not allowed to perform this action."));
        }
        
        return Task.FromResult(Result.Success());
    }
}