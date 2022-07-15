using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSA.Template.Audit.API.Data;
using MSA.Template.Audit.API.Models;

namespace MSA.Template.Audit.API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class EntityLogsController : ControllerBase
{
    private readonly ILogger<EntityLogsController> _logger;
    private readonly AuditDbContext _context;

    public EntityLogsController(ILogger<EntityLogsController> logger, AuditDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("{producer}/{entityName}/{entityId}")]
    public async Task<IActionResult> GetEntityLogsAsync(string producer, string entityName, string entityId)
    {
        var entityLogs = await _context.Set<EntityLog>()
            .Where(e => e.EntityName == entityName && e.EntityId == entityId)
            .ToListAsync();

        return Ok(entityLogs);
    }
}