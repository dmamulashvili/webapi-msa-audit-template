using Microsoft.EntityFrameworkCore;
using MSA.Template.Audit.API.Data.TypeConfigurations;

namespace MSA.Template.Audit.API.Data;

public class AuditDbContext : DbContext
{
    public AuditDbContext(DbContextOptions<AuditDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntityLogTypeConfiguration).Assembly);
    }
}