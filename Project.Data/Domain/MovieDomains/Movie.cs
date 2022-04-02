using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Domain.MovieDomains
{
    public class Movie : BaseEntity<long>, IAuditEntity
    {
        public string Title { get; set; }

        public string OriginalTitle { get; set; }

        public int IntegrationId { get; set; }

        public string Overview { get; set; }


        public DateTime InsertDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        public bool IsDeleted { get; set; }


        public virtual ICollection<MovieRate> MovieRates { get; set; }
        public virtual ICollection<MovieNote> MovieNotes { get; set; }
    }
}
