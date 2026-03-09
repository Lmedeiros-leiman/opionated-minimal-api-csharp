using TemplateNormal.Common.Api.Filters;
using TemplateNormal.Common.Data;

namespace TemplateNormal.Common.Api.Extensions;

public static class RouteHandlerBuilderEntityExtensions
{
    /// <summary>
    /// Adds a request validation filter to ensure the entity exists.
    /// </summary>
    public static RouteHandlerBuilder WithEnsureEntityExists<TEntity, TRequest>(this RouteHandlerBuilder builder, Func<TRequest, int?> idSelector)
        where TEntity : class, IEntity
    {
        return builder
            .AddEndpointFilterFactory((endpointFilterFactoryContext, next) => async context =>
            {
                var db = context.HttpContext.RequestServices.GetRequiredService<SqlDbContext>();
                var filter = new EnsureEntityExistsFilter<TRequest, TEntity>(db, idSelector);
                return await filter.InvokeAsync(context, next);
            })
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
