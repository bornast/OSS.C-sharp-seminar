using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Identity.Domain.Entities
{
    public class RefreshToken
    {
        // set as primary key(guid) in configuration
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Used { get; set; }
        public bool Invalidated { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
    }
}
