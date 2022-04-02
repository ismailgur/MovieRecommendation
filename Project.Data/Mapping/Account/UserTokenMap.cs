using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Data.Domain.Account;

namespace Project.Data.Mapping.Account
{
    public class UserTokenMap : IEntityTypeConfiguration<UserToken>
    {
        public void Map(EntityTypeBuilder<UserToken> entity)
        {
            entity.ToTable("UserTokens");
        }
    }
}
