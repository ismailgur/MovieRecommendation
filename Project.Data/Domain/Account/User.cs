using Project.Data.Domain.MovieDomains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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

        public DateTime InsertDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<MovieRate> MovieRates { get; set; }
    }
}
