using MGH.Domain.Entities;
using MGH.EF.Persistence.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGH.EF.Persistence.Configurations;

public class CarConfig : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        //table
        builder.ToTable(DatabaseTableName.Car, DatabaseSchema.GeneralSchema);


        //fields
        builder.HasKey(a => new { a.Id, a.Name });

        builder.Property(t => t.Name)
            .HasMaxLength(maxLength: 64)
            .IsRequired();

        builder.Property(t => t.ModelYear)
            .IsRequired()
            .HasMaxLength(maxLength: 4);
        
        
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