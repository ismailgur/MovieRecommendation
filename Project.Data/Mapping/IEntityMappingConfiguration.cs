using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Project.Data.Mapping
{
    public interface IEntityTypeConfiguration<TEntityType> where TEntityType : class
    {
        void Map(EntityTypeBuilder<TEntityType> builder);
    }
}
