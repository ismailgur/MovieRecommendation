using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Dto.MovieDtos
{
    public class MovieNoteDto
    {
        public long Id { get; set; }

        public string Note { get; set; }

        public DateTime InsertDateTime { get; set; }
    }

    public class MovieNoteSaveDto
    {
        public int IntegrationId { get; set; }

        public string Note { get; set; }
    }


    public class MovieScoreSaveDto
    {
        public int IntegrationId { get; set; }

        public int Score { get; set; }
    }
}
