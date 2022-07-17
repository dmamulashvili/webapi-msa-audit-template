using MassTransit;
using MSA.Template.Audit.Abstractions.Events;
using MSA.Template.Audit.Abstractions.Interfaces;
using MSA.Template.Audit.API.Data;
using MSA.Template.Audit.API.Models;

namespace MSA.Template.Audit.API.AuditEventHandlers;

public class EntityPropertyModifiedAuditEventHandler : IAuditEventHandler<EntityPropertyModifiedAuditEvent>
{
    private readonly AuditDbContext _context;

    public EntityPropertyModifiedAuditEventHandler(AuditDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<Batch<EntityPropertyModifiedAuditEvent>> context)
    {
        foreach (var message in context.Message)
        {
            var entityLog = new EntityLog(
                message.Host.Assembly!,
                message.Message.CorrelationId,
                message.Message.InitiatorId,
                message.Message.EntityName,
                message.Message.EntityId,
                message.Message.PropertyName,
                message.Message.PropertyOriginalValue,
                message.Message.PropertyCurrentValue,
                message.Message.CreationDate);

            _context.Set<EntityLog>().Add(entityLog);
        }

        await _context.SaveChangesAsync();
    }
}