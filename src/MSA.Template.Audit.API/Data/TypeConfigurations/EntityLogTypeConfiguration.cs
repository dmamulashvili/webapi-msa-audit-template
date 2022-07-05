using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSA.Template.Audit.API.Models;

namespace MSA.Template.Audit.API.Data.TypeConfigurations;

public class EntityLogTypeConfiguration : IEntityTypeConfiguration<EntityLog>
{
    public void Configure(EntityTypeBuilder<EntityLog> entityLogConfiguration)
    {
        entityLogConfiguration.ToTable(nameof(EntityLog));
        entityLogConfiguration.HasKey(p => p.Id);

        entityLogConfiguration.HasIndex(p => p.Producer);
        entityLogConfiguration.HasIndex(p => p.EntityId);
        entityLogConfiguration.HasIndex(p => p.EntityName);
        entityLogConfiguration.HasIndex(p => p.PropertyName);
        entityLogConfiguration.HasIndex(p => p.CreationDate);
    }
}