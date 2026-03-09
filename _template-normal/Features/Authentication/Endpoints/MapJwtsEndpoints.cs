using TemplateNormal.Features.Authentication.Endpoints;

namespace TemplateNormal;

public static partial class Endpoints
{
    static partial void MapJwtsEndpoints(IEndpointRouteBuilder authGroup)
    {
        authGroup.MapEndpoint<GetJwtsEndpoint>();
    }
}
