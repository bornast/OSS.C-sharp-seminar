using AutoMapper;
using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IBaseRepository<Role> _repo;
        private readonly IMapper _mapper;

        public RoleService(IBaseRepository<Role> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RoleDto>> GetAll()
        {
            var roles = await _repo.GetAllAsync();

            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }
    }
}
