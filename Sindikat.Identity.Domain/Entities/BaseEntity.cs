using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Identity.Domain.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
