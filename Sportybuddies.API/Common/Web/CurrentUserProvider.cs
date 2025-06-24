using System.Security.Claims;

namespace Sportybuddies.API.Common.Web;

public interface ICurrentUserProvider
{
    Guid GetCurrentUserId();
}

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    public Guid GetCurrentUserId()
    {
        var nameIdentifier = httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        Guid.TryParse(nameIdentifier, out var userId);

        return userId;
    }
}