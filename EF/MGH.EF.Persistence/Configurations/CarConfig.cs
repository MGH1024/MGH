using MGH.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGH.EF.Persistence.Configurations.Base;

public class EntityConfig : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        //table section
        builder.ToTable(DatabaseTableName.Car, DatabaseSchema.GeneralSchema);


        //fields section
        builder.HasKey(a => new { a.Id, a.Name });


        builder.Property(t => t.Name)
            .HasMaxLength(maxLength: 64)
            .IsRequired();

        builder.Property(t => t.ModelYear)
            .IsRequired()
            .HasMaxLength(maxLength: 4);
    }
}