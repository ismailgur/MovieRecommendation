using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Project.Data.Mapping.MovieMapps
{
    public class MovieNoteMap : IEntityTypeConfiguration<Domain.MovieDomains.MovieNote>
    {
        public void Map(EntityTypeBuilder<Domain.MovieDomains.MovieNote> entity)
        {
            entity.ToTable("MovieNotes");
        }
    }
}