using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Sindikat.Identity.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }

}
