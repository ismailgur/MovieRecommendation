using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Data.Domain.Account;

namespace Project.Data.Mapping.User
{
    public class UserMap : IEntityTypeConfiguration<Domain.Account.User>
    {
        public void Map(EntityTypeBuilder<Domain.Account.User> entity)
        {
            entity.ToTable("Users");

            entity.HasOne(e => e.UserToken)
                  .WithOne(ut => ut.User)
                  .HasForeignKey<UserToken>(ut => ut.UserId); // one-to-one association
        }
    }
}