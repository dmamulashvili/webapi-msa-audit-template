using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSA.Template.Audit.API.Data;
using MSA.Template.Audit.API.Models;

namespace MSA.Template.Audit.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EntityLogsController : ControllerBase
{
    private readonly ILogger<EntityLogsController> _logger;
    private readonly AuditDbContext _context;

    public EntityLogsController(ILogger<EntityLogsController> logger, AuditDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("{entityType}/{entityId}")]
    public async Task<IActionResult> GetEntityLogsAsync(string entityType, string entityId)
    {
        var entityLogs = await _context.Set<EntityLog>()
            .Where(e => e.EntityName == entityType && e.EntityId == entityId)
            .ToListAsync();

        return Ok(entityLogs);
    }
}