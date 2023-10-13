using MGH.EF.Persistence.Configurations.Base;
using MGH.EF.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGH.EF.Persistence.Configurations;

public class CommentConfig : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        //table
        builder.ToTable(DatabaseTableName.Comment, DatabaseSchema.GeneralSchema);


        //fields
        builder.Property(a => a.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .HasMaxLength(maxLength: 64)
            .IsRequired();

        builder.Property(t => t.Text)
            .HasMaxLength(maxLength: 512)
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

        builder.Property(a => a.CreatedBy)
            .HasDefaultValue("user");

        builder.Property(a => a.CreatedAt)
            .HasDefaultValueSql("GetDate()");
    }
}