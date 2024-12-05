using MGH.Identity.Configurations.Base;
using MGH.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGH.Identity.Configurations.Entities;

public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.ToTable(DatabaseTableName.UserLogin, DatabaseSchema.IdentitySchema);


        builder.Property(t => t.ProviderDisplayName)
            .HasMaxLength(512);
    }
}

