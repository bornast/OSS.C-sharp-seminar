using System.Collections.Generic;

namespace Sindikat.Identity.Application.Dtos
{
    public class UserForUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<UserClaimToUpdateDto> Claims { get; set; } = new List<UserClaimToUpdateDto>();
        public List<string> RoleIds { get; set; } = new List<string>();
    }

    public class UserClaimToUpdateDto
    {
        public int ClaimId { get; set; }
        public string ClaimValue { get; set; }
    }

}
