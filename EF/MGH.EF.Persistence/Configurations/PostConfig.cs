using MGH.Domain.Entities;
using MGH.Domain.Entities.EF;
using MGH.EF.Persistence.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGH.EF.Persistence.Configurations;

public class PostConfig : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        //table
        builder.ToTable(DatabaseTableName.Post, DatabaseSchema.GeneralSchema);


        //fields
        builder.Property(t => t.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Title)
            .HasMaxLength(maxLength: 64)
            .IsRequired();

        builder.Property(t => t.Text)
            .HasMaxLength(maxLength: 512);

        
        //constraint
        builder.HasIndex(t => t.Title)
            .IsUnique();
        
        //navigations
        builder.HasMany(a => a.Comments)
            .WithOne(a => a.Post)
            .HasForeignKey(a => a.PostId)
            .HasPrincipalKey(a => a.Id);
        
        builder
            .HasMany(e => e.Tags)
            .WithMany(e => e.Posts)
            .UsingEntity(
                "PostTag",
                l => l.HasOne(typeof(Tag)).WithMany().HasForeignKey("TagsId").HasPrincipalKey(nameof(Tag.Id)),
                r => r.HasOne(typeof(Post)).WithMany().HasForeignKey("PostsId").HasPrincipalKey(nameof(Post.Id)),
                j => j.HasKey("PostsId", "TagsId"));

        //public
        builder.Ignore(a => a.Row);
        builder.Ignore(a => a.PageSize);
        builder.Ignore(a => a.TotalCount);
        builder.Ignore(a => a.CurrentPage);
        
        builder.Ignore(a => a.ListItemText);
        builder.Ignore(a => a.ListItemTextForAdmins);
        
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
            .HasDefaultValueSql("GETDATE()");
    }
}