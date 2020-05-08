using AutoMapper;
using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Sindikat.Identity.Application.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Role, RoleDto>();

            CreateMap<ClaimForSaveDto, Claim>();

            CreateMap<Claim, ClaimDto>();

            CreateMap<RegisterDto, User>();

            CreateMap<User, UserForListDto>();

            CreateMap<UserForUpdateDto, User>()
            .AfterMap((src, dest) =>
            {
                // remove roles
                var rolesToRemove = dest.UserRoles.Where(x => !src.RoleIds.Contains(x.RoleId)).ToList();

                foreach (var roleToRemove in rolesToRemove)
                    dest.UserRoles.Remove(roleToRemove);

                // add roles
                var rolesToAdd = src.RoleIds.Where(id => !dest.UserRoles.Any(x => x.RoleId == id)).ToList()
                .Select(id => new UserRole { RoleId = id });

                foreach (var roleToAdd in rolesToAdd)
                    dest.UserRoles.Add(roleToAdd);

                // remove claims
                var claimsToRemove = dest.UserClaims
                .Where(x => !src.Claims.Select(x => x.ClaimId).Contains(x.ClaimId)).ToList();

                foreach (var claimToRemove in claimsToRemove)
                    dest.UserClaims.Remove(claimToRemove);

                // update claims
                foreach (var claim in dest.UserClaims.Where(x => src.Claims.Select(x => x.ClaimId).Contains(x.ClaimId)))
                {
                    var claimFromRequest = src.Claims.FirstOrDefault(x => x.ClaimId == claim.ClaimId);

                    if (claimFromRequest != null)
                        claim.ClaimValue = claimFromRequest.ClaimValue;
                }                

                // add claims
                var claimsToAdd = src.Claims.Where(c => !dest.UserClaims.Any(x => x.ClaimId == c.ClaimId)).ToList()
                .Select(c => new UserClaim { ClaimId = c.ClaimId, ClaimValue = c.ClaimValue });

                foreach (var claimToAdd in claimsToAdd)
                    dest.UserClaims.Add(claimToAdd);                                

            });            

            CreateMap<User, UserForDetailedDto>()
                .ForMember(x => x.Roles, opt =>
                opt.MapFrom(x => x.UserRoles.Select(x => new RoleDto { Id = x.Role.Id, Name = x.Role.Name })))
            .AfterMap((src, dest) =>
            {
                var claimsToAdd = new List<UserClaimDto>();
                if (src.UserClaims.Count > 0)
                {
                    foreach (var claim in src.UserClaims)
                    {
                        var claimDto = new UserClaimDto
                        {
                            Id = claim.Claim.Id,
                            Claim = claim.Claim.Name,
                            ClaimValue = claim.ClaimValue
                        };
                        claimsToAdd.Add(claimDto);
                    }
                    dest.Claims.AddRange(claimsToAdd);
                }

            });
        }
    }
}
