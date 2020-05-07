using AutoMapper;
using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Common.Exceptions;
using Sindikat.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IBaseRepository<Claim> _repo;
        private readonly IMapper _mapper;

        public ClaimService(IBaseRepository<Claim> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClaimDto>> GetAll()
        {
            var claims = await _repo.GetAllAsync();

            return _mapper.Map<IEnumerable<ClaimDto>>(claims);
        }

        public async Task<ClaimDto> GetOne(int id)
        {
            var claim = await _repo.FindAsync(id);

            if (claim == null)
                throw new NotFoundException();

            return _mapper.Map<ClaimDto>(claim);
        }

        public async Task Create(ClaimForSaveDto claimForSave)
        {
            var claim = _mapper.Map<Claim>(claimForSave);

            _repo.Persist(claim);

            await _repo.FlushAsync();
        }        

        public async Task Update(int claimId, ClaimForSaveDto claimForSave)
        {
            var claim = await _repo.FindAsync(claimId);

            _mapper.Map(claimForSave, claim);

            await _repo.FlushAsync();
        }

        public async Task Delete(int id)
        {
            var claim = await _repo.FindAsync(id);

            _repo.Delete(claim);

            await _repo.FlushAsync();
        }
    }
}
