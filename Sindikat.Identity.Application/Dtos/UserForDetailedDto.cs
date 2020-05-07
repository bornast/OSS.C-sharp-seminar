using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Identity.Application.Dtos
{
    public class UserForDetailedDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<UserClaimDto> Claims { get; set; } = new List<UserClaimDto>();
        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();

    }
}
