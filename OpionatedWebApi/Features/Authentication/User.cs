using OpionatedWebApi.Common.Data;

namespace OpionatedWebApi.Features.Authentication;

public class User : IEntity
{
    public int Id { get; private init; }
    public Guid ReferenceId { get; private init; } = Guid.NewGuid();
}
