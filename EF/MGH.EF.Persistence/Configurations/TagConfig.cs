﻿using MGH.EF.Persistence.Configurations.Base;
using MGH.EF.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGH.EF.Persistence.Configurations;

public class TagConfig : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        //table
        builder.ToTable(DatabaseTableName.Tag, DatabaseSchema.GeneralSchema);


        //fields
        builder.Property(a => a.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Title)
            .HasMaxLength(maxLength: 64)
            .IsRequired();

        
        //public
        builder.Property(t => t.CreatedBy)
            .IsRequired()
            .HasMaxLength(maxLength: 64);
        
        builder.Property(t => t.CreatedAt)
            .IsRequired();
        
        builder.Property(t => t.UpdatedBy)
            .HasMaxLength(maxLength: 64);
        
        builder.Property(t => t.UpdatedAt)
            .IsRequired(false);
        
        builder.Property(t => t.DeletedBy)
            .HasMaxLength(maxLength: 64);
        
        builder.Property(t => t.DeletedAt)
            .IsRequired(false);
    }
}