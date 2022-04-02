using System;
using System.Security.Claims;

namespace Project.Common.Extensions
{
    public static class IdentityExtensions
    {
        public static CurrentUser GetCurrentUser(this ClaimsPrincipal identity)
        {
            try
            {
                var displayName = identity.FindFirst("DisplayName").Value;
                var email = identity.FindFirst(ClaimTypes.Email).Value;
                var userId = long.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

                return new CurrentUser
                {
                    DisplayName = displayName,
                    Email = email,
                    UserId = userId,
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public class CurrentUser
    {
        public long UserId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}
