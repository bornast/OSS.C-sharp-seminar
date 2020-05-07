using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Identity.Application.Dtos
{
    public class UserClaimDto
    {
        public int Id { get; set; }
        public string Claim { get; set; }
        public string ClaimValue { get; set; }
    }
}
