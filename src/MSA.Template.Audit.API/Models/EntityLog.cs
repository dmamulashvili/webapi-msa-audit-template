namespace MSA.Template.Audit.API.Models;

public class EntityLog
{
    public EntityLog()
    {
    }

    public EntityLog(
        string producer,
        Guid correlationId,
        Guid initiatorId,
        string entityName,
        string entityId,
        string propertyName,
        string? propertyOriginalValue,
        string? propertyCurrentValue,
        DateTimeOffset creationDate)
    {
        Producer = producer;
        CorrelationId = correlationId;
        InitiatorId = initiatorId;
        EntityName = entityName;
        EntityId = entityId;
        PropertyName = propertyName;
        PropertyOriginalValue = propertyOriginalValue;
        PropertyCurrentValue = propertyCurrentValue;
        CreationDate = creationDate;
    }

    public Guid Id { get; private set; }
    public string Producer { get; private set; } = null!;
    public Guid CorrelationId { get; private set; }
    public Guid InitiatorId { get; private set; }
    public string EntityName { get; private set; } = null!;
    public string EntityId { get; private set; } = null!;
    public string PropertyName { get; private set; } = null!;
    public string? PropertyOriginalValue { get; private set; }
    public string? PropertyCurrentValue { get; private set; }
    public DateTimeOffset CreationDate { get; private set; }
}