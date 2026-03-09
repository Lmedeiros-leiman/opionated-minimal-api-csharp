using TemplateNormal.Common.Data;

namespace TemplateNormal.Features.Authentication;

public class User : IEntity
{
    public int Id { get; private init; }
    public Guid ReferenceId { get; private init; } = Guid.NewGuid();
}
