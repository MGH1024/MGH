﻿using MGH.Core.Infrastructure.Securities.Identity.Configurations.Base;
using MGH.Core.Infrastructure.Securities.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGH.Core.Infrastructure.Securities.Identity.Configurations.Entities;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(DatabaseTableName.Permission, DatabaseSchema.IdentitySchema);
        builder.Property(t => t.PermissionId)
           .IsRequired()
           .ValueGeneratedOnAdd();

        builder.Property(t => t.Title)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(t => t.Url)
          .HasMaxLength(256)
          .IsRequired();

        builder.Property(t => t.Description)
          .HasMaxLength(512);
    }
}

