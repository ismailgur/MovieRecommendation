using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Project.Data.Mapping.User
{
    public class UserMap : IEntityTypeConfiguration<Domain.Account.User>
    {
        public void Map(EntityTypeBuilder<Domain.Account.User> entity)
        {
            entity.ToTable("Users");
        }
    }
}