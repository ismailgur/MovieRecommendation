﻿using Project.Data.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Domain.MovieDomains
{
    public class MovieNote : BaseEntity<long>, IAuditEntity
    {
        public long MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; }


        public DateTime InsertDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
