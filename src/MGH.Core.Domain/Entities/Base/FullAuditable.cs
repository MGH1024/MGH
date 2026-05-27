using MGH.Core.Domain.Abstractions;
using MGH.Core.Domain.Abstractions.Auditing;

namespace MGH.Core.Domain.Entities.Base;

public abstract class FullAuditable<T> : IFullAuditable, IEntity<T>
{
    public T Id { get; protected set; }   // locked after creation

    public string CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedFromIp { get; set; }
    public DateTime? Deleted { get; set; }
    public string? DeletedBy { get; set; }
    public string? DeletedFromIp { get; set; }
    public DateTime? Modified { get; set; }
    public string? ModifiedBy { get; set; }
    public string? ModifiedFromIp { get; set; }

    protected FullAuditable(T id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    // For ORM materialization (e.g. EF Core) if needed, use a private parameterless constructor.
    // EF Core can still set the Id via reflection or a private constructor.
    protected FullAuditable()
    {
    }

    // Explicit interface implementation with guarded setter
    T IEntity<T>.Id
    {
        get => Id;
        set
        {
            // Allow setting only if Id is currently the default value (i.e., not yet set).
            // This permits the ORM to assign the Id during materialization while preventing
            // subsequent changes from outside the aggregate.
            if (!EqualityComparer<T>.Default.Equals(Id, default(T)))
                throw new InvalidOperationException("The Id of an entity cannot be changed once set.");

            Id = value;
        }
    }
}