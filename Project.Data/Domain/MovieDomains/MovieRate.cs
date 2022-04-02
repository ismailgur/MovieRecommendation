using Project.Data.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Domain.MovieDomains
{
    public class MovieRate
    {
        public long MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; }

        public int Score { get; set; }
    }
}
