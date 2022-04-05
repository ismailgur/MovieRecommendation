using Project.Data.Domain.MovieDomains;
using System;
using System.Collections.Generic;

namespace Project.Data.Domain.Account
{
    public class User : BaseEntity<long>, IAuditEntity
    {
        public User()
        {
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public virtual UserToken UserToken { get; set; }

        public string SerialNumber { get; set; }

        public string Password { get; set; }

        public DateTime InsertDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<MovieRate> MovieRates { get; set; }
    }
}
