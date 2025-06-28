namespace Sportybuddies.API.Data;

public class TransactionBehavior<TRequest, TResponse>(
    ApplicationDbContext dbContext)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next(cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return response;
    }
}