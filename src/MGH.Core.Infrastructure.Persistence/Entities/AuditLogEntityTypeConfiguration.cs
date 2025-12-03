using Microsoft.EntityFrameworkCore;
using MGH.Core.Infrastructure.Persistence.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGH.Core.Infrastructure.Persistence.Entities;

public class AuditLogEntityTypeConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable(BaseDatabaseTableName.AuditLog,BaseDatabaseSchemas.LogSchema);

        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.TableName)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.Action)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.Username)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.BeforeData)
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.AfterData)
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.Timestamp)
            .IsRequired();
    }
}