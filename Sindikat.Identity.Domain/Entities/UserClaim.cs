using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Identity.Domain.Entities
{
    public class UserClaim
    {
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public virtual Claim Claim { get; set; }
        public int ClaimId { get; set; }
        public string ClaimValue { get; set; }
    }
}
