using OpionatedWebApi.Features.Authentication.Endpoints;

namespace OpionatedWebApi;

public static partial class Endpoints
{
    static partial void MapJwtsEndpoints(IEndpointRouteBuilder authGroup)
    {
        authGroup.MapEndpoint<GetJwtsEndpoint>();
    }
}
