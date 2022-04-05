using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Dto.MovieDtos
{
    public class MovieDetailDto
    {
        public long Id { get; set; }

        public int IntegrationId { get; set; }

        public string Title { get; set; }

        public string OriginalTitle { get; set; }

        public string Image { get; set; }

        public string TopImage { get; set; }

        public string Overview { get; set; }

        public double? RateScoreAvg { get; set; }

        public int? RateScore { get; set; }

        public IList<MovieNoteDto> Notes { get; set; }
    }
}
