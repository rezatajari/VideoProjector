namespace Domain.Abstractions;

public abstract  class BaseEntity
{
    public Guid Id { get;protected set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsDeleted { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }
}