using System;

namespace Project.Data.Domain.Account
{
    public class UserToken : BaseEntity<long>
    {
        public string AccessTokenHash { get; set; }

        public DateTimeOffset AccessTokenExpiresDateTime { get; set; }

        public string RefreshTokenIdHash { get; set; }

        public DateTimeOffset RefreshTokenExpiresDateTime { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}
