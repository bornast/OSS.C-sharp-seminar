using Sindikat.Identity.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IClaimService
    {
        Task<IEnumerable<ClaimDto>> GetAll();
        Task<ClaimDto> GetOne(int id);
        Task<ClaimDto> Create(ClaimForSaveDto claimForSave);
        Task<ClaimDto> Update(int claimId, ClaimForSaveDto claimForSave);
        Task Delete(int id);
    }
}
