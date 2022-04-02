using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Project.Data.Mapping.MovieMapps
{
    public class MovieRateMap : IEntityTypeConfiguration<Domain.MovieDomains.MovieRate>
    {
        public void Map(EntityTypeBuilder<Domain.MovieDomains.MovieRate> entity)
        {
            entity.ToTable("MovieRates");

            entity.HasKey(e => new { e.UserId, e.MovieId });
        }
    }
}