using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Sindikat.Identity.Domain.Entities
{
    public class Role : IdentityRole
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
