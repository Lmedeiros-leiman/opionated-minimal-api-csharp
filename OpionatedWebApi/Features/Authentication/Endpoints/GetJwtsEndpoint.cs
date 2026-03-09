using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using OpionatedWebApi.Common.Api;

namespace OpionatedWebApi.Features.Authentication.Endpoints;

internal sealed class JwtsDocument
{
    [JsonPropertyName("keys")]
    public required JwtsKey[] Keys { get; init; }
}

internal sealed class JwtsKey
{
    [JsonPropertyName("kty")]
    public required string Kty { get; init; }

    [JsonPropertyName("alg")]
    public required string Alg { get; init; }

    [JsonPropertyName("use")]
    public required string Use { get; init; }

    [JsonPropertyName("issuer")]
    public string? Issuer { get; init; }

    [JsonPropertyName("audience")]
    public string? Audience { get; init; }

    [JsonPropertyName("pem")]
    public string? Pem { get; init; }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JwtsDocument))]
internal partial class JwtsJsonContext : JsonSerializerContext;

public sealed class GetJwtsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/jwts.json", Handle)
        .WithSummary("Returns public JWT configuration when ES256 key exposure is enabled")
        .WithName("GetJwts")
        .AllowAnonymous()
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

    private static Results<ContentHttpResult, NotFound> Handle(IConfiguration configuration)
    {
        const string jwtSection = "Authentication:Jwt";

        var mode = configuration["Authentication:Mode"];
        var exposePublicKey = configuration.GetValue<bool?>("Authentication:Jwt:Es256:ExposePublicKey") ?? false;

        if (!string.Equals(mode, "ES256", StringComparison.OrdinalIgnoreCase) || !exposePublicKey)
        {
            return TypedResults.NotFound();
        }

        var issuer = configuration[$"{jwtSection}:Issuer"];
        var audience = configuration[$"{jwtSection}:Audience"];
        var publicKeyPem = configuration[$"{jwtSection}:Es256:PublicKeyPem"];

        var payload = new JwtsDocument
        {
            Keys =
            [
                new JwtsKey
                {
                    Kty = "EC",
                    Alg = "ES256",
                    Use = "sig",
                    Issuer = issuer,
                    Audience = audience,
                    Pem = publicKeyPem
                }
            ]
        };

        var json = JsonSerializer.Serialize(payload, JwtsJsonContext.Default.JwtsDocument);

        return TypedResults.Content(json, "application/json");
    }
}
