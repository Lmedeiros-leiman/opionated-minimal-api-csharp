using OpionatedWebApi.Common.Data;
using OpionatedWebApi.DatabaseContext;
using OpionatedWebApi.Features.Authentication.Filters;

namespace OpionatedWebApi.Features.Authentication.Extensions;

public static class RouteHandlerBuilderAuthenticationExtensions
{
    /// <summary>
    /// Adds an ownership filter to ensure the current user owns the selected entity.
    /// </summary>
    public static RouteHandlerBuilder WithEnsureUserOwnsEntity<TEntity, TRequest>(this RouteHandlerBuilder builder, Func<TRequest, int> idSelector)
        where TEntity : class, IEntity, IOwnedEntity
    {
        return builder
            .AddEndpointFilterFactory((endpointFilterFactoryContext, next) => async context =>
            {
                var db = context.HttpContext.RequestServices.GetRequiredService<SqlDbContext>();
                var filter = new EnsureUserOwnsEntityFilter<TRequest, TEntity>(db, idSelector);
                return await filter.InvokeAsync(context, next);
            })
            .ProducesProblem(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status403Forbidden);
    }
}
