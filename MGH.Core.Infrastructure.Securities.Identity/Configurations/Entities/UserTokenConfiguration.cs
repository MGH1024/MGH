using MGH.Core.Infrastructure.Securities.Identity.Configurations.Base;
using MGH.Core.Infrastructure.Securities.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGH.Core.Infrastructure.Securities.Identity.Configurations.Entities;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable(DatabaseTableName.UserToken, DatabaseSchema.IdentitySchema);
    }
}
