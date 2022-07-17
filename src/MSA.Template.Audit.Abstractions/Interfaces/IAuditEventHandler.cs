using MassTransit;

namespace MSA.Template.Audit.Abstractions.Interfaces;

public interface IAuditEventHandler<in T> : IConsumer<Batch<T>>
    where T : BaseAuditEvent
{
    
}