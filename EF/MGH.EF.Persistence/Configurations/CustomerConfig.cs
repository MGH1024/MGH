using MGH.Domain.Entities;
using MGH.EF.Persistence.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGH.EF.Persistence.Configurations;

public class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        //table
        builder.ToTable(DatabaseTableName.Customer, DatabaseSchema.GeneralSchema);


        //fields
        builder.Property(t => t.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();


        builder.Property(t => t.FirstName)
            .HasMaxLength(maxLength: 64)
            .IsRequired();

        builder.Property(t => t.LastName)
            .HasMaxLength(maxLength: 128);

        //navigations
        
        
        //public
        builder.Ignore(a => a.Row);
        builder.Ignore(a => a.PageSize);
        builder.Ignore(a => a.TotalCount);
        builder.Ignore(a => a.CurrentPage);
        
        builder.Ignore(a => a.ListItemText);
        builder.Ignore(a => a.ListItemTextForAdmins);
        
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