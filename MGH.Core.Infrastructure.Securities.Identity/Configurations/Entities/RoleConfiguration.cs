using MGH.Core.Infrastructure.Securities.Identity.Configurations.Base;
using MGH.Core.Infrastructure.Securities.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGH.Core.Infrastructure.Securities.Identity.Configurations.Entities;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(DatabaseTableName.Role, DatabaseSchema.IdentitySchema);


        //seed data
        builder.HasData
            (
            new Role
            {
                Id = 1,
                Name = "Administrator",
                Description = "admin",
                NormalizedName = "administrator",
            },
            new Role
            {
                Id = 2,
                Name = "User",
                Description = "user",
                NormalizedName = "user"
            }
            );

        builder.Property(t => t.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Description)
            .HasMaxLength(256);



        builder.Property(t => t.Name)
             .HasMaxLength(256)
             .IsRequired();

        builder.Property(t => t.NormalizedName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(t => t.ConcurrencyStamp)
            .HasMaxLength(1024)
            .IsRequired();
    }
}

