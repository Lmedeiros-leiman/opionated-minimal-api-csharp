namespace OpionatedWebApi.Common.Data;

public interface IEntity
{
    // Used for database entities that have natural Keys.
    int Id { get; }
    // Used when data is received from an external source or when we need unordered data insertion.
    Guid ReferenceId { get; }
}

public interface IOwnedEntity
{
    int UserId { get; }
}