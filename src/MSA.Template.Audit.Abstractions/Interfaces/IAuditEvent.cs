namespace MSA.Template.Audit.Abstractions.Interfaces;

public interface IAuditEvent
{
    public Guid CorrelationId { get; }
    public Guid InitiatorId { get; }
    public DateTimeOffset CreationDate { get; }
}