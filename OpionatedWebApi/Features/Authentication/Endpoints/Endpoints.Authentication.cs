namespace OpionatedWebApi;

public static partial class Endpoints
{
    static partial void MapAuthenticationEndpoints(RouteGroupBuilder baseGroup)
    {
        var authGroup = baseGroup
            .WithTags("Auth")
            .MapPublicGroup("/.well-known");

        MapJwtsEndpoints(authGroup);
    }

    static partial void MapJwtsEndpoints(IEndpointRouteBuilder authGroup);
}
