using AutoMapper;
using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Domain.Entities;

namespace Sindikat.Identity.Application.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ClaimForSaveDto, Claim>();
            CreateMap<Claim, ClaimDto>();
        }
    }
}
