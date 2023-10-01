using MGH.Domain.Entities;
using MGH.EF.Persistence.Configurations.Base;
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
            .HasMaxLength(maxLength: 64);
        
        builder.Property(t => t.CreatedDate)
            .IsRequired();
        
        builder.Property(t => t.UpdatedBy)
            .HasMaxLength(maxLength: 64);
        
        builder.Property(t => t.UpdatedDate)
            .IsRequired(false);
        
        builder.Property(t => t.DeletedBy)
            .HasMaxLength(maxLength: 64);
        
        builder.Property(t => t.DeletedDate)
            .IsRequired(false);
    }
}