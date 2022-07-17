namespace MSA.Template.Audit.Abstractions.Interfaces;

public interface IAuditEventService
{
    Task AddEventAsync(BaseAuditEvent @event);
    Task PublishEventsAsync(CancellationToken cancellationToken);
}