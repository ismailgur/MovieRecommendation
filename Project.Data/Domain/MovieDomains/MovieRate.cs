using Project.Data.Domain.Account;

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
