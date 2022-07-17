using MSA.Template.Audit.Abstractions.Interfaces;

namespace MSA.Template.Audit.Abstractions;

public abstract class BaseAuditEvent : IAuditEvent
{
    public Guid CorrelationId { get; protected init; }
    public Guid InitiatorId { get; protected init; }
    public DateTimeOffset CreationDate { get; protected init; }
}