using MassTransit;
using MSA.Template.Audit;

namespace SharedKernel.Audit.Interfaces;

public interface IAuditEventHandler<in T> : IConsumer<Batch<T>>
    where T : BaseAuditEvent
{
    
}