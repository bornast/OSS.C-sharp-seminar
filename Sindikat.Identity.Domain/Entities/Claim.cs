using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Identity.Domain.Entities
{
    public class Claim : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
