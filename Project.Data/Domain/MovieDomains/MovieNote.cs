using Project.Data.Domain.Account;
using System;

namespace Project.Data.Domain.MovieDomains
{
    public class MovieNote : BaseEntity<long>, IAuditEntity
    {
        public long MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; }

        public string Note { get; set; }


        public DateTime InsertDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
