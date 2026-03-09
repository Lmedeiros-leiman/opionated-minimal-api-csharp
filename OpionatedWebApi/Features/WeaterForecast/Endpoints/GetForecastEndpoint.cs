using Microsoft.AspNetCore.Http.HttpResults;
using OpionatedWebApi.Common.Api;

namespace OpionatedWebApi.Features.WeaterForecast.Endpoints;


public class GetForecastEndpoint : IEndpoint {
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/", Handle)
        .WithSummary("Get the current weather forecast")
        .WithName("GetWeatherForecast")
        .Produces<Response[]>(StatusCodes.Status200OK);


    private static async Task<Ok<Response[]>> Handle(
        Request request,
        CancellationToken cancellationToken)
    {

        var summaries = new[] {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var forecast = Enumerable.Range(1, 5).Select(index =>
        new Response
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

        return TypedResults.Ok(forecast);
    }

    public record Response(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }


    public record Request();




}
