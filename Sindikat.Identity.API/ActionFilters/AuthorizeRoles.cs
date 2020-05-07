using Microsoft.AspNetCore.Authorization;
using Sindikat.Identity.Common.Enums;
using System;
using System.Linq;

namespace Sindikat.Identity.API.ActionFilters
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params Roles[] allowedRoles)
        {
            var allowedRolesAsStrings = allowedRoles.Select(x => Enum.GetName(typeof(Roles), x));
            Roles = string.Join(",", allowedRolesAsStrings);
        }
    }
}
