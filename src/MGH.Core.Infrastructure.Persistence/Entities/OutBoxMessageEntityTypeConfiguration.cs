using Microsoft.EntityFrameworkCore;
using MGH.Core.Infrastructure.Persistence.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGH.Core.Infrastructure.Persistence.Entities;

public class OutBoxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(BaseDatabaseTableName.Outbox, BaseDatabaseSchemas.LogSchema);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.Payload)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.Property(x => x.Error)
            .HasMaxLength(2000);

        builder.Property(x => x.OccurredOn)
            .IsRequired();

        builder.Property(x => x.ProcessedAt)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(x => x.ProcessedAt);
        builder.HasIndex(x => x.OccurredOn);
    }
}
