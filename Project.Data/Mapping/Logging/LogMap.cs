using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Data.Domain.Logging;

namespace Project.Data.Mapping.MovieMapps
{
    public class LogMap : IEntityTypeConfiguration<Log>
    {
        public void Map(EntityTypeBuilder<Log> entity)
        {
            entity.ToTable("Logs");
        }
    }
}