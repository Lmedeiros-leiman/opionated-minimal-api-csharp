using TemplateNormal.Features.WeaterForecast.Endpoints;
using TemplateNormal.Common.Api;

namespace TemplateNormal;


// Main file responsible for registering routes in the API.
// Remenber to either add an automatic route register,
// or simply manually register then here. not that big of a deal.
public static partial class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        RouteGroupBuilder BaseGroup = app.MapGroup("");

        //
        IEndpointRouteBuilder WeatherGroup = BaseGroup
            .WithTags("Weather")
            .MapPublicGroup("/weatherforecast")
            .MapEndpoint<GetForecastEndpoint>();

        MapAuthenticationEndpoints(BaseGroup);


    }


    //
    // Helper methods for creating groups.
    private static RouteGroupBuilder MapPublicGroup(this IEndpointRouteBuilder app, string? prefix = null)
    {
        return app.MapGroup(prefix ?? string.Empty)
            .AllowAnonymous();
    }


    private static RouteGroupBuilder MapAuthorizedGroup(this IEndpointRouteBuilder app, string? prefix = null)
    {

        return app.MapGroup(prefix ?? string.Empty)
            .RequireAuthorization();
    }


    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }

    static partial void MapAuthenticationEndpoints(RouteGroupBuilder baseGroup);
}
