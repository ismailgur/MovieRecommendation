using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Project.Data.Mapping.MovieMapps
{
    public class MovieMap : IEntityTypeConfiguration<Domain.MovieDomains.Movie>
    {
        public void Map(EntityTypeBuilder<Domain.MovieDomains.Movie> entity)
        {
            entity.ToTable("Movies");
            entity.Property(e => e.Title).HasMaxLength(450).IsRequired();
        }
    }
}