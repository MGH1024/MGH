using MGH.Core.Infrastructure.Securities.Identity.Configurations.Base;
using MGH.Core.Infrastructure.Securities.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGH.Core.Infrastructure.Securities.Identity.Configurations.Entities;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable(DatabaseTableName.UseRole, DatabaseSchema.IdentitySchema);

        builder.HasData
            (new UserRole
            {
                RoleId=1,
                UserId=1,
            });
    }
}
